using Innowise.Clinic.Offices.Persistence.Models;
using Innowise.Clinic.Offices.Persistence.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Innowise.Clinic.Offices.Persistence.Repositories.Implementations;

public class OfficeRepository : IOfficeRepository
{
    private readonly IMongoCollection<OfficeModel> _offices;

    public OfficeRepository(IOptions<MongoDbConfiguration> dbConfiguration)
    {
        var client = new MongoClient(dbConfiguration.Value.ConnectionString);
        var database = client.GetDatabase(dbConfiguration.Value.Database);
        _offices = database.GetCollection<OfficeModel>(dbConfiguration.Value.Collection);
    }

    public async Task<IEnumerable<OfficeModel>> GetOfficesAsync()
    {
        return await (await _offices.FindAsync(Builders<OfficeModel>.Filter.Empty)).ToListAsync();
    }

    public async Task<OfficeModel?> GetOfficeAsync(Guid id)
    {
        return await (await _offices.FindAsync(Builders<OfficeModel>.Filter.Eq(x => x.Id, id)))
            .SingleOrDefaultAsync();
    }

    public async Task<Guid> CreateOfficeAsync(OfficeModel newOffice)
    {
        await _offices.InsertOneAsync(newOffice);
        return newOffice.Id;
    }

    public async Task UpdateOfficeAsync(OfficeModel updatedOffice)
    {
        await _offices.ReplaceOneAsync(
            Builders<OfficeModel>.Filter.Eq(x => x.Id, updatedOffice.Id),
            updatedOffice);
    }

    public async Task DeleteOfficeAsync(OfficeModel officeToDelete)
    {
        await _offices.DeleteOneAsync(Builders<OfficeModel>.Filter.Eq(x => x.Id, officeToDelete.Id));
    }
}