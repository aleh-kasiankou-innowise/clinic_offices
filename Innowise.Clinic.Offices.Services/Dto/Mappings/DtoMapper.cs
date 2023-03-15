using Innowise.Clinic.Offices.Persistence.Models;

namespace Innowise.Clinic.Offices.Services.Dto.Mappings;

/// <summary>
/// Class containing extensions for mapping models and dtos.
/// </summary>
public static class DtoMapper
{
    public static OfficeModel ToModel(this OfficeUploadDto officeUploadDto, string? imageUrl)
    {
        return new OfficeModel
        {
            OfficeStatus = officeUploadDto.OfficeStatus,
            OfficeAddress = officeUploadDto.OfficeAddress,
            RegistryPhone = officeUploadDto.RegistryPhone,
            ImageUrl = imageUrl
        };
    }

    /// <summary>
    /// Converts office update DTO to Office model.
    /// </summary>
    /// <param name="officeUpdateDto">Updated office info.</param>
    /// <param name="officeId">Id of the saved office.</param>
    /// <param name="imageUrl">URL of the office image.</param>
    /// <returns>Office model with all the fields mapped.</returns>
    public static OfficeModel ToModel(this OfficeUpdateDto officeUpdateDto, Guid officeId, string? imageUrl)
    {
        return new OfficeModel
        {
            Id = officeId,
            OfficeStatus = officeUpdateDto.OfficeStatus,
            OfficeAddress = officeUpdateDto.OfficeAddress,
            RegistryPhone = officeUpdateDto.RegistryPhone,
            ImageUrl = imageUrl
        };
    }
}