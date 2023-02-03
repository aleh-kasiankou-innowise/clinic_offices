using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Innowise.Clinic.Offices.Constants;
using Innowise.Clinic.Offices.Dto;
using Innowise.Clinic.Offices.Persistence.Models;
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
            OfficeAddress = new OfficeAddressModel()
            {
                City = "City",
                HouseNumber = "HouseNumber",
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
                 x.OfficeAddress.HouseNumber == office.OfficeAddress.HouseNumber &&
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
}