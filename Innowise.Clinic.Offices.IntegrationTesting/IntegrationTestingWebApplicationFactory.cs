using System;
using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Innowise.Clinic.Offices.Api;
using Innowise.Clinic.Offices.Persistence;
using Innowise.Clinic.Offices.Services.OfficeRepository.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Innowise.Clinic.Offices.IntegrationTesting;

public class IntegrationTestingWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly IContainer _dbContainer = new ContainerBuilder()
        .WithImage("mongo:latest")
        .WithEnvironment("MONGO_INITDB_ROOT_USERNAME", "offices_api_user").WithEnvironment(
            "MONGO_INITDB_ROOT_PASSWORD", "secureM0ngoPassword").WithPortBinding(25594, 27017)
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(27017)).Build();

    private const string TestConnectionString =
        "mongodb://offices_api_user:secureM0ngoPassword@localhost:25594";

    public IntegrationTestingWebApplicationFactory()
    {
    }

    public T UseDb<T>(Func<IOfficeRepository, T> func)
    {
        using (var scope = Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<IOfficeRepository>();
            return func(db);
        }
    }

    public T UseConfiguration<T>(Func<IConfiguration, T> func)
    {
        using (var scope = Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var config = scopedServices.GetRequiredService<IConfiguration>();
            return func(config);
        }
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.Configure<MongoDbConfiguration>(x =>
            {
                x.ConnectionString = TestConnectionString;
            });
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await base.DisposeAsync();
    }
}