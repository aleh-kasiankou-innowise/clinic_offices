using Innowise.Clinic.Offices.Dto;
using Innowise.Clinic.Offices.Dto.RabbitMq;
using Innowise.Clinic.Offices.Persistence;
using Innowise.Clinic.Offices.Persistence.Models;
using Innowise.Clinic.Offices.Services.OfficeRepository.Interfaces;
using Innowise.Clinic.Offices.Services.RabbitMqPublisher;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Innowise.Clinic.Offices.Services.OfficeRepository.Implementations;

public class OfficeRepository : IOfficeRepository
{
    private readonly IMongoCollection<OfficeModel> _offices;
    private readonly IRabbitMqPublisher _rabbitMqPublisher;

    public OfficeRepository(IOptions<MongoDbConfiguration> dbConfiguration, IRabbitMqPublisher rabbitMqPublisher)
    {
        _rabbitMqPublisher = rabbitMqPublisher;
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

    public OfficeModel? GetOffice(Guid id)
    {
        return _offices.Find(Builders<OfficeModel>.Filter.Eq(x => x.Id, id))
            .SingleOrDefault();
    }

    public async Task<OfficeModel?> GetOfficeAsync(Guid id)
    {
        return await (await _offices.FindAsync(Builders<OfficeModel>.Filter.Eq(x => x.Id, id)))
            .SingleOrDefaultAsync();
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
        _rabbitMqPublisher.NotifyAboutOfficeChange(
            new OfficeChangeTask(OfficeChange.Add,
                new OfficeAddressDto(newOffice.Id, newOffice.OfficeAddress.ToString()))
        );
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
        _rabbitMqPublisher.NotifyAboutOfficeChange(
            new OfficeChangeTask(OfficeChange.Add,
                new OfficeAddressDto(newOffice.Id, newOffice.OfficeAddress.ToString()))
        );
        return newOffice.Id;
    }

    public void UpdateOffice(OfficeModel currentOfficeModel, OfficeDto officeUpdateDto)
    {
        var updatedOffice = new OfficeModel
        {
            OfficeStatus = officeUpdateDto.OfficeStatus,
            OfficeAddress = officeUpdateDto.OfficeAddress,
            RegistryPhone = officeUpdateDto.RegistryPhone,
            Image = officeUpdateDto.Image
        };
        _offices.ReplaceOne(
            Builders<OfficeModel>.Filter.Eq(x => x.Id, currentOfficeModel.Id), updatedOffice
        );
        _rabbitMqPublisher.NotifyAboutOfficeChange(
            new OfficeChangeTask(OfficeChange.Update,
                new OfficeAddressDto(currentOfficeModel.Id, officeUpdateDto.OfficeAddress.ToString()))
        );
    }

    public async Task UpdateOfficeAsync(OfficeModel currentOfficeModel, OfficeDto officeUpdateDto)
    {
        var updatedOffice = new OfficeModel
        {
            Id = currentOfficeModel.Id,
            OfficeStatus = officeUpdateDto.OfficeStatus,
            OfficeAddress = officeUpdateDto.OfficeAddress,
            RegistryPhone = officeUpdateDto.RegistryPhone,
            Image = officeUpdateDto.Image
        };
        await _offices.ReplaceOneAsync(
            Builders<OfficeModel>.Filter.Eq(x => x.Id, currentOfficeModel.Id),
            updatedOffice
        );
        _rabbitMqPublisher.NotifyAboutOfficeChange(
            new OfficeChangeTask(OfficeChange.Update,
                new OfficeAddressDto(currentOfficeModel.Id, officeUpdateDto.OfficeAddress.ToString()))
        );
    }

    public void DeleteOffice(OfficeModel officeToDelete)
    {
        _offices.DeleteOne(Builders<OfficeModel>.Filter.Eq(x => x.Id, officeToDelete.Id));
        _rabbitMqPublisher.NotifyAboutOfficeChange(
            new OfficeChangeTask(OfficeChange.Remove,
                new OfficeAddressDto(officeToDelete.Id, null))
        );
    }

    public async Task DeleteOfficeAsync(OfficeModel officeToDelete)
    {
        await _offices.DeleteOneAsync(Builders<OfficeModel>.Filter.Eq(x => x.Id, officeToDelete.Id));
        _rabbitMqPublisher.NotifyAboutOfficeChange(
            new OfficeChangeTask(OfficeChange.Remove,
                new OfficeAddressDto(officeToDelete.Id, null))
        );
    }
}