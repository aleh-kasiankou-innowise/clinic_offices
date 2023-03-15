using Innowise.Clinic.Offices.Persistence.Models;
using Innowise.Clinic.Offices.Services.Dto;

namespace Innowise.Clinic.Offices.Services.OfficeService.Interfaces;

public interface IOfficeService
{

    Task<IEnumerable<OfficeModel>> GetOfficesAsync();
    Task<OfficeModel?> GetOfficeAsync(Guid id);
    Task<Guid> CreateOfficeAsync(OfficeUploadDto officeDto);
    Task UpdateOfficeAsync(OfficeModel currentOfficeModel, OfficeUpdateDto officeUpdateDto);
    Task DeleteOfficeAsync(OfficeModel officeToDelete);
}