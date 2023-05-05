using System.Reflection;
using System.Text;
using Innowise.Clinic.Offices.Persistence;
using Innowise.Clinic.Offices.Persistence.Repositories.Implementations;
using Innowise.Clinic.Offices.Persistence.Repositories.Interfaces;
using Innowise.Clinic.Offices.Services.Dto;
using Innowise.Clinic.Offices.Services.OfficeService.Implementations;
using Innowise.Clinic.Offices.Services.OfficeService.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace Innowise.Clinic.Offices.Extensions;

public static class ConfigurationExtensions
{
    public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(opts =>
        {
            opts.SwaggerDoc("v1", new OpenApiInfo { Title = "Office API", Version = "v1" });
            opts.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
                $"Innowise.Clinic.Offices.Api.xml"));
            opts.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
                $"{Assembly.GetAssembly(typeof(OfficeAddress))?.GetName().Name}.xml"));
            opts.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
                $"{Assembly.GetAssembly(typeof(OfficeUploadDto))?.GetName().Name}.xml"));

            opts.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "JWT Authorization header using the Bearer scheme."
            });

            opts.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" }
                    },
                    new string[] { }
                }
            });
        });

        return services;
    }

    public static IServiceCollection ConfigureRepositories(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<MongoDbConfiguration>(configuration.GetSection("MongoDb"));
        services.AddScoped<IOfficeRepository, OfficeRepository>();
        services.AddScoped<IOfficeService, OfficeService>();
        return services;
    }

    public static IServiceCollection ConfigureSecurity(this IServiceCollection services)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    Environment.GetEnvironmentVariable("JWT__KEY") ?? throw new
                        InvalidOperationException()))
            };
        });
        return services;
    }

    public static IServiceCollection ConfigureCrossServiceCommunication(this IServiceCollection services,
        IConfiguration configuration)
    {
        var rabbitMqConfig = configuration.GetSection("RabbitConfigurations");
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqConfig["HostName"], h =>
                {
                    h.Username(rabbitMqConfig["UserName"]);
                    h.Password(rabbitMqConfig["Password"]);
                });
                cfg.ConfigureEndpoints(context);
            });
        });
        return services;
    }
    
    public static WebApplicationBuilder ConfigureSerilog(this WebApplicationBuilder builder)
    {
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(builder.Configuration["ElasticSearchHost"]))
            {
                AutoRegisterTemplate = true,
                OverwriteTemplate = true,
                IndexFormat = $"clinic.offices-{0:yy.MM}",
                BatchAction = ElasticOpType.Index,
                DetectElasticsearchVersion = true,
            })
            .WriteTo.Console()
            .CreateLogger();

        Log.Logger = logger;
        builder.Host.UseSerilog(logger);
        return builder;
    }
}