using Innowise.Clinic.Offices.Persistence.Models;

namespace Innowise.Clinic.Offices.Dto;

public class OfficeDto
{
    public string RegistryPhone { get; set; } = null!;
    public OfficeStatus OfficeStatus { get; set; }
    public OfficeAddressModel OfficeAddress { get; set; } = null!;
    public Byte[] Image { get; set; } = null!;
}