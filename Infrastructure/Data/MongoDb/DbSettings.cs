namespace Infrastructure.Data.MongoDb;

public class DbSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string Rosters { get; set; } = null!;
}