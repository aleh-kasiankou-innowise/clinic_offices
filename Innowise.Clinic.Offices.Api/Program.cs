using Innowise.Clinic.Offices.Configuration;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureSecurity();
builder.Services.ConfigureRepositories(builder.Configuration);
builder.Services.ConfigureCrossServiceCommunication(builder.Configuration);

var app = builder.Build();

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