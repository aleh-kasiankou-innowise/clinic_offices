using Innowise.Clinic.Offices.Persistence.Models;

namespace Innowise.Clinic.Offices.Persistence.Repositories.Interfaces;

public interface IOfficeRepository
{
    Task<IEnumerable<OfficeModel>> GetOfficesAsync();
    Task<OfficeModel?> GetOfficeAsync(Guid id);
    Task<Guid> CreateOfficeAsync(OfficeModel newOffice);
    Task UpdateOfficeAsync(OfficeModel updatedOffice);
    Task DeleteOfficeAsync(OfficeModel officeToDelete);
}