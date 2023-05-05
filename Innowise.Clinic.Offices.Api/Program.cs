using Innowise.Clinic.Offices.Api.Middleware;
using Innowise.Clinic.Offices.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureSecurity();
builder.Services.ConfigureRepositories(builder.Configuration);
builder.Services.ConfigureCrossServiceCommunication(builder.Configuration);
builder.Services.AddSingleton<ExceptionHandlingMiddleware>();
builder.ConfigureSerilog();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

Log.Information("The Offices service is starting");
app.Run();
Log.Information("The Offices service is stopping");
await Log.CloseAndFlushAsync();

namespace Innowise.Clinic.Offices.Api
{
    public partial class Program
    {
    }
}