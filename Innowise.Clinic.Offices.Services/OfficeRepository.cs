using Innowise.Clinic.Offices.Dto;
using Innowise.Clinic.Offices.Persistence.Models;
using Innowise.Clinic.Offices.Services.Interfaces;

namespace Innowise.Clinic.Offices.Services;

public class OfficeRepository : IOfficeRepository
{
    public IEnumerable<OfficeModel> GetOffices()
    {
        throw new NotImplementedException();
    }

    public OfficeModel GetOffice(Guid id)
    {
        throw new NotImplementedException();
    }

    public Guid CreateOffice(OfficeDto officeDto)
    {
        throw new NotImplementedException();
    }

    public void UpdateOffice(Guid id, OfficeDto officeDto)
    {
        throw new NotImplementedException();
    }

    public void DeleteOffice(Guid id)
    {
        throw new NotImplementedException();
    }
}