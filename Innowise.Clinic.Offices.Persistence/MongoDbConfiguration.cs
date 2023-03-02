namespace Innowise.Clinic.Offices.Persistence;

public class MongoDbConfiguration
{
    public string ConnectionString { get; set; } = null!;
    public string Database { get; set; } = null!;
    public string Collection { get; set; } = null!;
}