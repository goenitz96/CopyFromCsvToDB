
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using CSV.Database;
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
            await db.ExecuteAsync(query, new
            {
                row.SiteId, row.startHour, row.finalHour, row.bytesUpload, row.bytesDownload
            }, commandType: CommandType.StoredProcedure);
        }
        return data;
    }

    public async Task<IList<Hourly>> csvReadFileAndCopyToDB()
    {
        var filePath = @"/Users/omegajohn/Documents/Files/Hourly_data.csv";
        var data = new List<Hourly>();
        using (var streamReader = new StreamReader(filePath))
        {
            using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
            {
                data = csvReader.GetRecords<Hourly>().ToList();
            }
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