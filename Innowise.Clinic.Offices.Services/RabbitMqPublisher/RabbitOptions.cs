namespace Innowise.Clinic.Offices.Services.RabbitMqPublisher;

public class RabbitOptions
{
    public string HostName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string OfficesProfilesExchangeName { get; set; }
    public string OfficeChangeRoutingKey { get; set; }
}