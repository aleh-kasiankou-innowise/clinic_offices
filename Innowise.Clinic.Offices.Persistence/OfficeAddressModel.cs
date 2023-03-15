using System.ComponentModel.DataAnnotations;

namespace Innowise.Clinic.Offices.Persistence;

/// <summary>
/// Represents an office address.
/// </summary>
public class OfficeAddress
{
    /// <summary>
    /// Office location city in a text format.
    /// </summary>
    /// <example>New York</example>
    [Required]
    public string City { get; init; }
    
    /// <summary>
    /// Office location street in a text format.
    /// </summary>
    /// <example>Columbia Ave</example>
    [Required]
    public string Street { get; init; }
    
    /// <summary>
    /// Office location house number in a text format.
    /// </summary>
    /// <example>168</example>
    [Required]
    public string BuildingNumber { get; init; }
    
    /// <summary>
    /// Office number in a text format.
    /// </summary>
    /// <example>35</example>
    public string? OfficeNumber { get; init; }

    public override string ToString()
    {
        var addressBase = $"{City}, {Street} {BuildingNumber}";
        return OfficeNumber is null ? addressBase : addressBase + ", office " + OfficeNumber;
    }
}