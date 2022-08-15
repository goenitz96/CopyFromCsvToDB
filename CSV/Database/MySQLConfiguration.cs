namespace CSV.Database;

public class MySQLConfiguration
{
    public string connection { get; set; }
    
    public MySQLConfiguration(string connection)
    {
        this.connection = connection;
    }
}