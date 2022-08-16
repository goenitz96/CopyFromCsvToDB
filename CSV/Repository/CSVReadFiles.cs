
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using CSV.Database;
using CSV.Helpers;
using CSV.Interfaces;
using CSV.Models;
using CsvHelper;
using Dapper;
using MySql.Data.MySqlClient;
using Renci.SshNet;

namespace CSV.Repository;

public class CSVReadFiles : ICsvReadFile
{
    //private readonly string connection;
    private MySQLConfiguration con;
    
    public CSVReadFiles(IConfiguration config, MySQLConfiguration con)
    {
        this.con = con;
        //this.connection = config.GetConnectionString("SQL");
    }

    protected MySqlConnection dbConnection()
    {
        return new MySqlConnection(con.connection);
    }

    public async Task<IList<Hourly>> saveTheData()
    {
        var db = dbConnection();
        await db.OpenAsync();
        var data = await csvReadFileAndCopyToDB();
        var query = @"hourly_data_csv";
        foreach (var row in data)
        {
            var parameters = new DynamicParameters();
            parameters.Add("id", row.SiteId);
            parameters.Add("start_hour", row.startHour);
            parameters.Add("final_hour", row.finalHour);
            parameters.Add("minutes_used", row.minutesUsed);
            parameters.Add("bytes_upload", row.bytesUpload);
            parameters.Add("bytes_download", row.bytesDownload);
            
            await db.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
        }
        return data;
    }

    public void downloadFtpCsvFile(string userName, string password, string host, string fileName, string ftpPath,
        string downloadPath)
    {
        var fullPath = $"{ftpPath}/{fileName}";
        using (SftpClient ftp = new SftpClient(new PasswordConnectionInfo(host, userName, password)))
        {
            ftp.Connect();
            using (Stream stream = File.Create(downloadPath + @"/" + fileName))
            {
                ftp.DownloadFile(fullPath, stream);
            }
            ftp.Disconnect();
        }
    }

    public void deleteFile(string downloadPath)
    {
        var fullPath = $"{downloadPath}";
        try
        {
            if (File.Exists(downloadPath))
            {
                File.Delete(fullPath);
            }
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<IList<Hourly>> csvReadFileAndCopyToDB()
    {
        var filePath = @"C:\Users\jespinozam\Downloads\Files\Hourly_Data.csv";
        var data = new List<Hourly>();
        try
        {
            using (var streamReader = new StreamReader(filePath))
            {
                using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    await csvReader.ReadAsync();
                    csvReader.ReadHeader();
                    csvReader.Context.RegisterClassMap<HourlyDataMap>();
                    data = csvReader.GetRecords<Hourly>().ToList();
                }
            }
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

        return data;
    }
}