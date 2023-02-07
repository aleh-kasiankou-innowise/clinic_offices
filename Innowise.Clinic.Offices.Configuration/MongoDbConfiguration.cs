namespace Innowise.Clinic.Offices.Configuration;

public class MongoDbConfiguration
{
    public string ConnectionString { get; set; } = null!;
    public string Database { get; set; } = null!;
    public string Collection { get; set; } = null!;
}