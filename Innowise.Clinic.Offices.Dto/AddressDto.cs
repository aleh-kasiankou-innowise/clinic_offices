namespace Innowise.Clinic.Offices.Dto;

public class AddressDto
{
    public string City { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string HouseNumber { get; set; } = null!;
    public string? OfficeNumber { get; set; }
}