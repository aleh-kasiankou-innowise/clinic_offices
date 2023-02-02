using Innowise.Clinic.Offices.Configuration;
using Innowise.Clinic.Offices.Dto;
using Innowise.Clinic.Offices.Persistence.Interfaces;
using Innowise.Clinic.Offices.Persistence.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Innowise.Clinic.Offices.Persistence;

public class OfficeRepository : IOfficeRepository
{
    private readonly MongoClient _client;
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<OfficeModel> _offices;

    public OfficeRepository(IOptions<MongoDbConfiguration> dbConfiguration)
    {
        _client = new MongoClient(dbConfiguration.Value.ConnectionString);
        _database = _client.GetDatabase(dbConfiguration.Value.Database);
        _offices = _database.GetCollection<OfficeModel>(dbConfiguration.Value.Collection);
    }


    public IEnumerable<OfficeModel> GetOffices()
    {
        return _offices.Find(Builders<OfficeModel>.Filter.Empty).ToList();
    }

    public async Task<IEnumerable<OfficeModel>> GetOfficesAsync()
    {
        return await (await _offices.FindAsync(Builders<OfficeModel>.Filter.Empty)).ToListAsync();
    }

    public OfficeModel GetOffice(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<OfficeModel> GetOfficeAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Guid CreateOffice(OfficeDto officeDto)
    {
        throw new NotImplementedException();
    }

    public async Task<Guid> CreateOfficeAsync(OfficeDto officeDto)
    {
        throw new NotImplementedException();
    }

    public void UpdateOffice(Guid id, OfficeDto officeDto)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateOfficeAsync(Guid id, OfficeDto officeDto)
    {
        throw new NotImplementedException();
    }

    public void DeleteOffice(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteOfficeAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}