using CSV.Interfaces;
using CSV.Models;
using CSV.Database;
using MySqlConnector;


namespace CSV.Repository;

public class UpdateTerminalsRepository : IJupiterService
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
    
    public async Task<IList<Terminal>> getDataFromJupiter()
    {
        var db = dbConnection();
        await db.OpenAsync();
        var data = await jupiterService.getDataFromJupiter();
        var query = "";
        //Logic here
        return data;
    }
}