using System.ComponentModel.DataAnnotations;

namespace Innowise.Clinic.Offices.Persistence.Models;

/// <summary>
/// Represents an office address.
/// </summary>
public class OfficeAddressModel
{
    /// <summary>
    /// Office location city in a text format.
    /// </summary>
    /// <example>New York</example>
    [Required]
    public string City { get; set; }
    
    /// <summary>
    /// Office location street in a text format.
    /// </summary>
    /// <example>Columbia Ave</example>
    [Required]
    public string Street { get; set; }
    
    /// <summary>
    /// Office location house number in a text format.
    /// </summary>
    /// <example>168</example>
    [Required]
    public string HouseNumber { get; set; }
    
    /// <summary>
    /// Office number in a text format.
    /// </summary>
    /// <example>35</example>
    public string? OfficeNumber { get; set; }
    
}