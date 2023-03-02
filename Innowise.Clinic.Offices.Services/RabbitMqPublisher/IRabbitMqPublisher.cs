using Innowise.Clinic.Offices.Dto.RabbitMq;

namespace Innowise.Clinic.Offices.Services.RabbitMqPublisher;

public interface IRabbitMqPublisher
{
    void NotifyAboutOfficeChange(OfficeChangeTask officeChangeTask);
}