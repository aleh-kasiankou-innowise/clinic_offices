namespace Innowise.Clinic.Offices.Dto.RabbitMq;

public record OfficeChangeTask(OfficeChange TaskType, OfficeAddressDto OfficeAddress);

public record OfficeAddressDto(Guid OfficeId, string? Address);