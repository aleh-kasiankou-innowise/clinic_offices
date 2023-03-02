using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Innowise.Clinic.Offices.Shared;
using Innowise.Clinic.Offices.Dto;
using Innowise.Clinic.Offices.Persistence.Enums;
using Innowise.Clinic.Offices.Persistence.Models;
using Innowise.Clinic.Offices.Shared.Constants;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Innowise.Clinic.Offices.IntegrationTesting;

public class OfficesControllerIntegrationTests : IClassFixture<IntegrationTestingWebApplicationFactory>
{
    private readonly IntegrationTestingWebApplicationFactory _factory;
    private readonly HttpClient _httpClient;

    public OfficesControllerIntegrationTests(IntegrationTestingWebApplicationFactory factory)
    {
        _factory = factory;
        _httpClient = _factory.CreateClient(new WebApplicationFactoryClientOptions()
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task TestCreatedOfficesAreReturned_Ok()
    {
        // Arrange

        var office = new OfficeDto()
        {
            OfficeStatus = OfficeStatus.Active,
            OfficeAddress = new OfficeAddress()
            {
                City = "City",
                BuildingNumber = "HouseNumber",
                Street = "Street",
                OfficeNumber = "OfficeNumber"
            },
            RegistryPhone = "2564892"
        };

        var createdObjId = await _factory.UseDb(x => x.CreateOfficeAsync(office));

        // Act

        var response = await _httpClient.GetAsync(ControllerRoutes.OfficesControllerRoute);
        var retrievedCollection = await response.Content.ReadFromJsonAsync<List<OfficeModel>>();

        // Assert

        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(retrievedCollection);
        Assert.NotEmpty(retrievedCollection);
        Assert.Contains(retrievedCollection,
            x => x.Id == createdObjId && x.OfficeStatus == office.OfficeStatus &&
                 x.OfficeAddress.OfficeNumber == office.OfficeAddress.OfficeNumber &&
                 x.OfficeAddress.BuildingNumber == office.OfficeAddress.BuildingNumber &&
                 x.OfficeAddress.City == office.OfficeAddress.City &&
                 x.OfficeAddress.Street == office.OfficeAddress.Street && x.RegistryPhone == office.RegistryPhone);
    }

    [Fact]
    public async Task TestEmptyListOfOfficesIsReturnedWithNoDataInDb_Ok()
    {
        // Arrange

        var response = await _httpClient.GetAsync(ControllerRoutes.OfficesControllerRoute);
        var createdOffices = await response.Content.ReadFromJsonAsync<List<OfficeModel>>();

        Debug.Assert(createdOffices != null, nameof(createdOffices) + " != null");
        foreach (var office in createdOffices)
        {
            await _httpClient.DeleteAsync(ControllerRoutes.OfficesControllerRoute + "/" + office.Id);
        }

        // Act

        response = await _httpClient.GetAsync(ControllerRoutes.OfficesControllerRoute);
        var retrievedCollection = await response.Content.ReadFromJsonAsync<List<OfficeModel>>();

        // Assert

        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(retrievedCollection);
        Assert.Empty(retrievedCollection);
    }

    [Fact]
    public async Task TestNewValidOfficeCreated_Ok()
    {
        // Arrange

        var office = new OfficeDto()
        {
            OfficeStatus = OfficeStatus.Active,
            OfficeAddress = new OfficeAddress()
            {
                City = "City",
                BuildingNumber = "HouseNumber",
                Street = "Street",
                OfficeNumber = "OfficeNumber"
            },
            RegistryPhone = "2564892",
            Image = Encoding.UTF8.GetBytes("this is an image converted to bytes")
        };

        // Act

        var response = await _httpClient.PostAsJsonAsync(ControllerRoutes.OfficesControllerRoute, office);
        var generatedObjectId = await response.Content.ReadFromJsonAsync<Guid>();

        // Assert

        Assert.True(response.IsSuccessStatusCode);
        Assert.NotEqual(Guid.Empty, generatedObjectId);
        var registeredOffices = (await _factory.UseDb(async x => await x.GetOfficesAsync())).ToList();

        Assert.Contains(registeredOffices, x => OfficeModelEqualsOfficeDto(x, office, generatedObjectId));
    }

    [Fact]
    public async Task TestExistingOfficeDetailsReturned_Ok()
    {
        // Arrange

        var office = new OfficeDto()
        {
            OfficeStatus = OfficeStatus.Active,
            OfficeAddress = new OfficeAddress()
            {
                City = "City",
                BuildingNumber = "HouseNumber",
                Street = "Street",
                OfficeNumber = "OfficeNumber"
            },
            RegistryPhone = "2564892",
            Image = Encoding.UTF8.GetBytes("this is an image converted to bytes")
        };

        var createdOfficeId = await _factory.UseDb(x => x.CreateOfficeAsync(office));

        // Act

        var response = await _httpClient.GetAsync(ControllerRoutes.OfficesControllerRoute + $"/{createdOfficeId}");
        var createdOffice = await response.Content.ReadFromJsonAsync<OfficeModel>();

        // Assert

        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(createdOffice);
        Assert.True(OfficeModelEqualsOfficeDto(createdOffice, office, createdOfficeId));
    }


    private bool OfficeModelEqualsOfficeDto(OfficeModel officeModel, OfficeDto officeDtoToCompare, Guid createdObjId)
    {
        return officeModel.Id == createdObjId && officeModel.OfficeStatus == officeDtoToCompare.OfficeStatus &&
               officeModel.OfficeAddress.OfficeNumber == officeDtoToCompare.OfficeAddress.OfficeNumber &&
               officeModel.OfficeAddress.BuildingNumber == officeDtoToCompare.OfficeAddress.BuildingNumber &&
               officeModel.OfficeAddress.City == officeDtoToCompare.OfficeAddress.City &&
               officeModel.OfficeAddress.Street == officeDtoToCompare.OfficeAddress.Street &&
               officeModel.RegistryPhone == officeDtoToCompare.RegistryPhone &&
               officeModel.Image.SequenceEqual(officeDtoToCompare.Image);
    }
}