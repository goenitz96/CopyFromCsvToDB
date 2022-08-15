
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
                    //Defaulter<long> parserToInt = new Defaulter<long>();
                    data = csvReader.GetRecords<Hourly>().ToList();
                    /*
                    foreach (var row in data)
                    {
                        data.Add(new Hourly
                        {
                            SiteId = csvReader.GetField("SiteID"),
                            startHour = Convert.ToDateTime(csvReader.GetField("Start hour")),
                            finalHour = Convert.ToDateTime(csvReader.GetField("Final hour")),
                            minutesUsed = Convert.ToInt32(csvReader.GetField("Minutes connected")),
                            bytesUpload = Convert.ToInt32(csvReader.GetField("Uplink VolumeUsage(bits)")),
                            bytesDownload = Convert.ToInt32(csvReader.GetField<long>("Downlink VolumeUsage(bits)", parserToInt))
                        });
                        if (parserToInt.GetLastError() != null)
                        {
                            string error = "Error: " + parserToInt.GetOffendingValue();
                            Console.WriteLine(error);
                        }
                    }
                    */
                }
            }
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

        return data;
    }
    
    
    /*
    public async Task<IList<Hourly>> getAllData()
    {
        var data = await csvReadFileAndCopyToDB();
        using (var con = new SqlConnection(connection))
        {
            using (var cmd = new SqlCommand("hourly_data_csv", con))
            {
                await con.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;
                var dt = new DataTable();
                dt.Columns.Add("SiteID", typeof(string));
                dt.Columns.Add("StartHour", typeof(DateTime));
                dt.Columns.Add("FinalHour", typeof(DateTime));
                dt.Columns.Add("MinutesUsed", typeof(int));
                dt.Columns.Add("BytesUpload", typeof(int));
                dt.Columns.Add("BytesDownload", typeof(int));

                foreach (var row in data)
                {
                    dt.Rows.Add(row.SiteId, row.startHour, row.finalHour, row.minutesUsed,
                        row.bytesUpload, row.bytesDownload);
                }
                var reader = await cmd.ExecuteReaderAsync();
                while (reader.Read())
                {
                    var SiteID = reader.GetString(0);
                    var StartHour = reader.GetDateTime(1);
                    var FinalHour = reader.GetDateTime(2);
                    var MinutesUsed = reader.GetInt32(3);
                    var BytesUpload = reader.GetInt32(4);
                    var BytesDownload = reader.GetInt32(5);
                    data.Append(new Hourly
                    {
                        SiteId = SiteID,
                        startHour = StartHour,
                        finalHour = FinalHour,
                        minutesUsed = MinutesUsed,
                        bytesUpload = BytesUpload,
                        bytesDownload = BytesDownload
                    });
                }
            }
            await con.CloseAsync();
        }
        return data;
    }
    */
}