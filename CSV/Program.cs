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
builder.Services.HangFireConfiguration(config);
builder.Services.AddHangfireServer();

/*Injection of cron jobs service*/
var recurringJob = builder.Services.BuildServiceProvider().GetService<IRecurringJobManager>();
var downloadFileJob = builder.Services.BuildServiceProvider().GetService<ICsvReadFile>();
var copyToDbJob = builder.Services.BuildServiceProvider().GetService<ICsvReadFile>();
var deleteFileJob = builder.Services.BuildServiceProvider().GetService<ICsvReadFile>();

/*Cron Jobs Schedule*/
recurringJob.AddOrUpdate("download-file-csv", () => 
    downloadFileJob.downloadFtpCsvFile(ftpServer, ftpPath, ftpFile, downloadPath, userName, password), 
    Cron.Hourly(10));

recurringJob.AddOrUpdate("copy-to-db", () => 
    copyToDbJob.saveTheData(), 
    Cron.Hourly(15));

recurringJob.AddOrUpdate("delete-file-csv", () => 
    deleteFileJob.deleteFile(downloadPath), Cron.Hourly(20));


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