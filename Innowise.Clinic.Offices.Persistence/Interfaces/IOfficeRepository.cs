using Innowise.Clinic.Offices.Dto;
using Innowise.Clinic.Offices.Persistence.Models;

namespace Innowise.Clinic.Offices.Persistence.Interfaces;

public interface IOfficeRepository
{
    IEnumerable<OfficeModel> GetOffices();

    Task<IEnumerable<OfficeModel>> GetOfficesAsync();
    OfficeModel? GetOffice(Guid id);
    Task<OfficeModel?> GetOfficeAsync(Guid id);

    Guid CreateOffice(OfficeDto officeDto);

    Task<Guid> CreateOfficeAsync(OfficeDto officeDto);


    void UpdateOffice(OfficeModel currentOfficeModel, OfficeDto officeUpdateDto);

    Task UpdateOfficeAsync(OfficeModel currentOfficeModel, OfficeDto officeUpdateDto);


    void DeleteOffice(OfficeModel officeToDelete);

    Task DeleteOfficeAsync(OfficeModel officeToDelete);
}