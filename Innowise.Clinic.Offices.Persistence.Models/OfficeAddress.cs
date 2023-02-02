using MongoDB.Bson.Serialization.Attributes;

namespace Innowise.Clinic.Offices.Persistence.Models;

public class OfficeAddressModel
{
    public string City { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string HouseNumber { get; set; } = null!;
    public string? OfficeNumber { get; set; }
    
}