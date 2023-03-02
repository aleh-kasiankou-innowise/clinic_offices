using Innowise.Clinic.Offices.Dto;
using Innowise.Clinic.Offices.Persistence.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace Innowise.Clinic.Offices.Persistence.Models;

/// <summary>
/// Represents an office registered in the system.
/// </summary>
public class OfficeModel
{
    /// <summary>
    /// Office Id.
    /// </summary>
    [BsonId]
    public Guid Id { get; set; }
    
    
    /// <summary>Phone number in a text format.</summary>
    /// <example>(555) 555-1234</example>>
    public string RegistryPhone { get; set; }
    
    /// <summary>Office status.</summary>
    /// <example>1</example>>
    public OfficeStatus OfficeStatus { get; set; }
    /// <summary>
    /// Office address.
    /// </summary>
    public OfficeAddress OfficeAddress { get; set; }

    /// <summary>
    /// Office photo in a binary format.
    /// </summary>
    /// <example>000000010010010010000000011110000000</example>
    public Byte[] Image { get; set; }
}