using System.Data;
using System.Globalization;
using System.Text.Json.Nodes;
using CSV.Interfaces;
using CSV.Models;
using CSV.Database;
using Dapper;
using MySqlConnector;
using Nancy.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace CSV.Repository;

public class UpdateTerminalsRepository : IUpdateTerminalsService
{
    private MySQLConfiguration con;
    private IJupiterService jupiterService;

    public UpdateTerminalsRepository(MySQLConfiguration con, IJupiterService jupiterService)
    {
        this.con = con;
        this.jupiterService = jupiterService;
    }

    protected MySqlConnection dbConnection()
    {
        return new MySqlConnection(con.connection);
    }

    public async Task<IList<Terminal>> UpdateTerminals()
    {
        var db = dbConnection();
        await db.OpenAsync();
        var data = await jupiterService.getDataFromJupiterOld();
        var query = "insertDataFromJupiter";
        try
        {
            foreach (var row in data)
            {
                row.properties = JsonConvert.SerializeObject(row);
                var parameters = new DynamicParameters();
                parameters.Add("device", row.deviceid);
                parameters.Add("property", row.properties);
                parameters.Add("loc", row.gatewayID);
                
                await db.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
            }
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        return data;
    }

    public async Task<IList<TerminalAppend>> UpdateTerminalsAppend()
    {
        var db = dbConnection();
        await db.OpenAsync();
        var appendData = await jupiterService.getDataFromJupiterNew();
        var query = "insertDataFromJupiterNew";
        try{
            
            foreach (var rowAppend in appendData)
            {
                var lon = rowAppend.latitude;
                var lat = rowAppend.longitude;
                rowAppend.longitude = lon;
                rowAppend.latitude = lat;
                rowAppend.properties = JsonConvert.SerializeObject(rowAppend);
                
                var parameters = new DynamicParameters();
                parameters.Add("id", rowAppend.id);
                parameters.Add("esnNew", rowAppend.esn);
                parameters.Add("lat", rowAppend.latitude);
                parameters.Add("lon", rowAppend.longitude);
                parameters.Add("property", rowAppend.properties);
                
                await db.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
            }
            
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        return appendData;
    }

    public async Task AllInOneJob()
    {
        try
        {
            await UpdateTerminals();
            await UpdateTerminalsAppend();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}