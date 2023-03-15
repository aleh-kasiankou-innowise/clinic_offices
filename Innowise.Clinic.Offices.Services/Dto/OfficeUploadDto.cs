using System.ComponentModel.DataAnnotations;
using Innowise.Clinic.Offices.Persistence;
using Innowise.Clinic.Offices.Shared.Enums;
using Microsoft.AspNetCore.Http;

namespace Innowise.Clinic.Offices.Services.Dto;

/// <summary>
/// Represents an office.
/// </summary>
public class OfficeUploadDto
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
    [Required]
    public OfficeAddress OfficeAddress { get; set; }

    /// <summary>
    /// Office photo.
    /// </summary>
    /// <example>000000010010010010000000011110000000</example>
    public IFormFile Image { get; set; }
}