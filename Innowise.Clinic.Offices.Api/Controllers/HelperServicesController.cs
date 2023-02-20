using Innowise.Clinic.Offices.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Innowise.Clinic.Offices.Api.Controllers;

[ApiController]
[Route("{controller}")]
public class HelperServicesController : ControllerBase
{
    private readonly IOfficeRepository _officesRepository;


    public HelperServicesController(IOfficeRepository officesRepository)
    {
        _officesRepository = officesRepository;
    }

    [HttpGet("ensure-exists/office/{id:guid}")]
    public async Task<IActionResult> EnsureOfficeExists([FromRoute] Guid id)
    {
        await _officesRepository.GetOfficeAsync(id);
        return Ok();
    }
}