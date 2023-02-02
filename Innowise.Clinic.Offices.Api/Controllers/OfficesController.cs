using Innowise.Clinic.Offices.Constants;
using Innowise.Clinic.Offices.Dto;
using Innowise.Clinic.Offices.Services.Interfaces;
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
    public IActionResult GetListOfOffices()
    {
        return Ok(_officesRepository.GetOffices());
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetOffice([FromRoute] Guid id)
    {
        return Ok(_officesRepository.GetOffice(id));
    }
    
    [HttpPost]
    public IActionResult CreateOffice([FromBody] OfficeDto office)
    {
        return Ok(_officesRepository.CreateOffice(office).ToString());
    }
    
    [HttpPut("{id:guid}")]
    public IActionResult UpdateOffice([FromRoute] Guid id, [FromBody] OfficeDto office)
    {
        _officesRepository.UpdateOffice(id, office);
        return Ok();
    }
    
    
    [HttpDelete("{id:guid}")]
    public IActionResult DeleteOffice([FromRoute] Guid id)
    {
        _officesRepository.DeleteOffice(id);
        return NoContent();
    }
    


}