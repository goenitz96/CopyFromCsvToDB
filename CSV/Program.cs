using CSV.Database;
using CSV.Interfaces;
using CSV.Repository;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
var mysql = config.GetConnectionString("MySQL");

var MySqlConnection = new MySQLConfiguration(mysql);
builder.Services.AddSingleton(MySqlConnection);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICsvReadFile, CSVReadFiles>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();