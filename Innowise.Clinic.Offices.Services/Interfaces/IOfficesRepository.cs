using Innowise.Clinic.Offices.Dto;
using Innowise.Clinic.Offices.Persistence.Models;

namespace Innowise.Clinic.Offices.Services.Interfaces;

public interface IOfficeRepository
{
    IEnumerable<OfficeModel> GetOffices();
    OfficeModel GetOffice(Guid id);

    Guid CreateOffice(OfficeDto officeDto);

    void UpdateOffice(Guid id, OfficeDto officeDto);

    void DeleteOffice(Guid id);

}