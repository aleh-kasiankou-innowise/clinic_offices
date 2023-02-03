using System.ComponentModel.DataAnnotations;
using Innowise.Clinic.Offices.Persistence.Models;

namespace Innowise.Clinic.Offices.Dto;

/// <summary>
/// Represents an office.
/// </summary>
public class OfficeDto
{
    /// <summary>Phone number in a text format.</summary>
    /// <example>(555) 555-1234</example>
    [Required]
    public string RegistryPhone { get; set; }

    /// <summary>Office status.</summary>
    /// <example>1</example>>
    [Required]
    public OfficeStatus OfficeStatus { get; set; }

    /// <summary>
    /// Office address.
    /// </summary>
    public OfficeAddressModel OfficeAddress { get; set; }

    /// <summary>
    /// Office photo in a binary format.
    /// </summary>
    /// <example>000000010010010010000000011110000000</example>
    public Byte[] Image { get; set; }
}