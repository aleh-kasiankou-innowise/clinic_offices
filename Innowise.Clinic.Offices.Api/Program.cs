using System.Reflection;
using Innowise.Clinic.Offices.Configuration;
using Innowise.Clinic.Offices.Dto;
using Innowise.Clinic.Offices.Persistence;
using Innowise.Clinic.Offices.Persistence.Interfaces;
using Innowise.Clinic.Offices.Persistence.Models;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(ctx =>
{
    ctx.SwaggerDoc("v1", new OpenApiInfo { Title = "Office API", Version = "v1" });
    ctx.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, 
        $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
    ctx.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, 
        $"{Assembly.GetAssembly(typeof(OfficeAddressModel))?.GetName().Name}.xml"));
    ctx.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, 
        $"{Assembly.GetAssembly(typeof(OfficeDto))?.GetName().Name}.xml"));
});
builder.Services.Configure<MongoDbConfiguration>(builder.Configuration.GetSection("MongoDb"));
builder.Services.AddScoped<IOfficeRepository, OfficeRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

#pragma warning disable CS1591
public partial class Program
#pragma warning restore CS1591
{
}