using System.Reflection;
using System.Text;
using Innowise.Clinic.Offices.Dto;
using Innowise.Clinic.Offices.Persistence;
using Innowise.Clinic.Offices.Persistence.Models;
using Innowise.Clinic.Offices.Services;
using Innowise.Clinic.Offices.Services.OfficeRepository.Implementations;
using Innowise.Clinic.Offices.Services.OfficeRepository.Interfaces;
using Innowise.Clinic.Offices.Services.RabbitMqPublisher;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Innowise.Clinic.Offices.Configuration;

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
                $"{Assembly.GetAssembly(typeof(OfficeDto))?.GetName().Name}.xml"));

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
        services.Configure<RabbitOptions>(configuration.GetSection("RabbitConfigurations"));
        services.AddSingleton<IRabbitMqPublisher, RabbitMqPublisher>();
        return services;
    }
}