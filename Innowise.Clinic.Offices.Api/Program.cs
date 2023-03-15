using Innowise.Clinic.Offices.Api.Middleware;
using Innowise.Clinic.Offices.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureSecurity();
builder.Services.ConfigureRepositories(builder.Configuration);
builder.Services.ConfigureCrossServiceCommunication(builder.Configuration);
builder.Services.AddSingleton<ExceptionHandlingMiddleware>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


namespace Innowise.Clinic.Offices.Api
{
    public partial class Program
    {
    }
}