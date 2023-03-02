using System.Text.Json;
using Innowise.Clinic.Offices.Dto.RabbitMq;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Innowise.Clinic.Offices.Services.RabbitMqPublisher;

public class RabbitMqPublisher : IRabbitMqPublisher
{
    private readonly IModel _channel;
    private readonly RabbitOptions _rabbitOptions;

    public RabbitMqPublisher(IOptions<RabbitOptions> rabbitConfig)
    {
        _rabbitOptions = rabbitConfig.Value;
        var factory = new ConnectionFactory
        {
            HostName = _rabbitOptions.HostName, UserName = _rabbitOptions.UserName, Password = _rabbitOptions.Password
        };
        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();
        DeclareOfficesProfilesExchange();
    }


    public void NotifyAboutOfficeChange(OfficeChangeTask officeChangeTask)
    {
        var body = JsonSerializer.SerializeToUtf8Bytes(officeChangeTask);
        _channel.BasicPublish(exchange: _rabbitOptions.OfficesProfilesExchangeName,
            routingKey: _rabbitOptions.OfficeChangeRoutingKey,
            basicProperties: null,
            body: body);
    }

    private void DeclareOfficesProfilesExchange()
    {
        _channel.ExchangeDeclare(exchange: _rabbitOptions.OfficesProfilesExchangeName,
            type: ExchangeType.Topic);
    }
}