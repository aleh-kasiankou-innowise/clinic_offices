using Innowise.Clinic.Offices.Configuration;
using Innowise.Clinic.Offices.Dto;
using Innowise.Clinic.Offices.Persistence.Interfaces;
using Innowise.Clinic.Offices.Persistence.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Innowise.Clinic.Offices.Persistence;

public class OfficeRepository : IOfficeRepository
{
    private readonly IMongoCollection<OfficeModel> _offices;

    public OfficeRepository(IOptions<MongoDbConfiguration> dbConfiguration)
    {
        var client = new MongoClient(dbConfiguration.Value.ConnectionString);
        var database = client.GetDatabase(dbConfiguration.Value.Database);
        _offices = database.GetCollection<OfficeModel>(dbConfiguration.Value.Collection);
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
        return _offices.Find(Builders<OfficeModel>.Filter.Eq(x => x.Id, id))
            .Single();
    }

    public async Task<OfficeModel> GetOfficeAsync(Guid id)
    {
        return await (await _offices.FindAsync(Builders<OfficeModel>.Filter.Eq(x => x.Id, id)))
            .SingleAsync();
    }

    public Guid CreateOffice(OfficeDto officeDto)
    {
        var newOffice = new OfficeModel
        {
            OfficeStatus = officeDto.OfficeStatus,
            OfficeAddress = officeDto.OfficeAddress,
            RegistryPhone = officeDto.RegistryPhone,
            Image = officeDto.Image
        };


        _offices.InsertOne(newOffice);


        return newOffice.Id;
    }

    public async Task<Guid> CreateOfficeAsync(OfficeDto officeDto)
    {
        var newOffice = new OfficeModel
        {
            OfficeStatus = officeDto.OfficeStatus,
            OfficeAddress = officeDto.OfficeAddress,
            RegistryPhone = officeDto.RegistryPhone,
            Image = officeDto.Image
        };


        await _offices.InsertOneAsync(newOffice);


        return newOffice.Id;
    }

    public void UpdateOffice(Guid id, OfficeDto officeDto)
    {
        var updatedOffice = new OfficeModel
        {
            OfficeStatus = officeDto.OfficeStatus,
            OfficeAddress = officeDto.OfficeAddress,
            RegistryPhone = officeDto.RegistryPhone,
            Image = officeDto.Image
        };

        _offices.ReplaceOne(Builders<OfficeModel>.Filter.Eq(x => x.Id, id), updatedOffice);

    }

    public async Task UpdateOfficeAsync(Guid id, OfficeDto officeDto)
    {
        var updatedOffice = new OfficeModel
        {
            Id = id,
            OfficeStatus = officeDto.OfficeStatus,
            OfficeAddress = officeDto.OfficeAddress,
            RegistryPhone = officeDto.RegistryPhone,
            Image = officeDto.Image
        };

        await _offices.ReplaceOneAsync(Builders<OfficeModel>.Filter.Eq(x => x.Id, id), updatedOffice);
    }

    public void DeleteOffice(Guid id)
    {
        _offices.DeleteOne(Builders<OfficeModel>.Filter.Eq(x => x.Id, id));
    }

    public async Task DeleteOfficeAsync(Guid id)
    {
        await _offices.DeleteOneAsync(Builders<OfficeModel>.Filter.Eq(x => x.Id, id));
    }
}