using System.Security.Cryptography.X509Certificates;
using CSV.Database;
using CSV.Helpers;
using CSV.Interfaces;
using CSV.Repository;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);
/*Environment variables*/
var config = builder.Configuration;
var mysql = config.GetConnectionString("MySQL");
var userName = config.GetConnectionString("FtpUser");
var password = config.GetConnectionString("FtpPassword");
var ftpServer = config.GetConnectionString("FtpServer");
var ftpPath = config.GetConnectionString("FtpPath");
var ftpFile = config.GetConnectionString("FtpFile");
var downloadPath = config.GetConnectionString("DownloadPath");

/*Injection of database*/
var MySqlConnection = new MySQLConfiguration(mysql);
builder.Services.AddSingleton(MySqlConnection);

/*Injection of services*/
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICsvReadFile, CSVReadFiles>();
builder.Services.AddScoped<IUpdateTerminalsService, UpdateTerminalsRepository>();
builder.Services.HangFireConfiguration(config);
builder.Services.AddHangfireServer();

/* Injection of jupiter service*/
builder.Services.AddHttpClient<IJupiterService, JupiterRepository>(x =>
{
    x.BaseAddress = new Uri(config.GetConnectionString("JupiterURL"));
});
builder.Services.AddScoped<IJupiterService, JupiterRepository>();

/*Injection of cron jobs service*/
var recurringJob = builder.Services.BuildServiceProvider().GetService<IRecurringJobManager>();
var downloadFileJob = builder.Services.BuildServiceProvider().GetService<ICsvReadFile>();
var copyToDbJob = builder.Services.BuildServiceProvider().GetService<ICsvReadFile>();
var insertIntoHourlyBillings = builder.Services.BuildServiceProvider().GetService<ICsvReadFile>();
var deleteFileJob = builder.Services.BuildServiceProvider().GetService<ICsvReadFile>();
var updateTerminalsJob = builder.Services.BuildServiceProvider().GetService<IUpdateTerminalsService>();

/*Cron Jobs Schedule*/
recurringJob.AddOrUpdate("download-file-csv", () => 
    downloadFileJob.downloadFtpCsvFile(userName, password, ftpServer, ftpFile, ftpPath), 
    Cron.Hourly(10));

recurringJob.AddOrUpdate("copy-to-db", () => 
    copyToDbJob.saveTheData(), 
    Cron.Hourly(13));

recurringJob.AddOrUpdate("insert-into-hourly-billings", () => 
    insertIntoHourlyBillings.insertIntoHourlyBilling(), Cron.Hourly(16));

recurringJob.AddOrUpdate("delete-file-csv", () => 
    deleteFileJob.deleteFile(), Cron.Hourly(18));

recurringJob.AddOrUpdate("update-terminals-full", () =>
    updateTerminalsJob.AllInOneJob(), Cron.Daily(0,0));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHangfireDashboard();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();