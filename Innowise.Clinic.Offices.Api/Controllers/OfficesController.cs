using Innowise.Clinic.Offices.Constants;
using Innowise.Clinic.Offices.Dto;
using Innowise.Clinic.Offices.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Innowise.Clinic.Offices.Api.Controllers;

[ApiController]
[Route(ControllerRoutes.OfficesControllerRoute)]
public class OfficesController : ControllerBase
{
    private readonly IOfficeRepository _officesRepository;

    public OfficesController(IOfficeRepository officeRepository, IOfficeRepository officesRepository)
    {
        _officesRepository = officesRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetListOfOffices()
    {
        return Ok(await _officesRepository.GetOfficesAsync());
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetOffice([FromRoute] Guid id)
    {
        return Ok(await _officesRepository.GetOfficeAsync(id));
    }

    [HttpPost]
    public async Task<IActionResult> CreateOffice([FromBody] OfficeDto office)
    {
        return Ok((await _officesRepository.CreateOfficeAsync(office)).ToString());
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateOffice([FromRoute] Guid id, [FromBody] OfficeDto office)
    {
        await _officesRepository.UpdateOfficeAsync(id, office);
        return Ok();
    }


    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteOffice([FromRoute] Guid id)
    {
        await _officesRepository.DeleteOfficeAsync(id);
        return NoContent();
    }
}