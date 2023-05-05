using System.ComponentModel.DataAnnotations;
using Innowise.Clinic.Offices.Persistence;
using Innowise.Clinic.Offices.Shared.Enums;
using Microsoft.AspNetCore.Http;

namespace Innowise.Clinic.Offices.Services.Dto;

/// <summary>
/// Updated office information.
/// </summary>
public class OfficeUpdateDto
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
    /// New office image.
    /// </summary>
    public IFormFile? NewImage { get; set; }

    /// <summary>
    /// Flag indicating whether the saved photo should be deleted.
    /// </summary>
    public bool IsDeleteImage { get; set; } = false;
}