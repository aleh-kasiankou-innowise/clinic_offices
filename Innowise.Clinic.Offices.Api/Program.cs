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
builder.Services.ConfigureSwagger();
builder.Services.ConfigureSecurity();
builder.Services.Configure<MongoDbConfiguration>(builder.Configuration.GetSection("MongoDb"));
builder.Services.AddScoped<IOfficeRepository, OfficeRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


public partial class Program
{
}
