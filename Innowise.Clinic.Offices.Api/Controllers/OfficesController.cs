using Innowise.Clinic.Offices.Constants;
using Innowise.Clinic.Offices.Dto;
using Innowise.Clinic.Offices.Persistence.Interfaces;
using Innowise.Clinic.Offices.Persistence.Models;
using Microsoft.AspNetCore.Mvc;

namespace Innowise.Clinic.Offices.Api.Controllers;

/// <summary>
/// Office controller.
/// It provides endpoints to create, get, update and delete clinic offices.
/// </summary>
[ApiController]
[Route(ControllerRoutes.OfficesControllerRoute)]
public class OfficesController : ControllerBase
{
    private readonly IOfficeRepository _officesRepository;

#pragma warning disable CS1591
    public OfficesController(IOfficeRepository officeRepository, IOfficeRepository officesRepository)
#pragma warning restore CS1591
    {
        _officesRepository = officesRepository;
    }

    /// <summary>
    /// Gets the list of all the offices registered in the system.
    /// </summary>
    /// <returns>The list of offices with all the office details.</returns>
    /// <response code="200">Success. Returns the list of offices.</response>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OfficeModel>))]
    public async Task<IActionResult> GetListOfOffices()
    {
        return Ok(await _officesRepository.GetOfficesAsync());
    }

    /// <summary>Gets the individual office by its Id.</summary>
    /// <param name="id">The office ID. Ids are represented in the GUID (UUID) format.</param>
    /// <returns>The office with the specified ID.</returns>
    /// <response code="200">Success. The office has been found and returned.</response>
    /// <response code="404">Failure. The office with the provided id is not found.</response>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OfficeModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(void))]
    public async Task<IActionResult> GetOffice([FromRoute] Guid id)
    {
        var office = await _officesRepository.GetOfficeAsync(id);

        if (office != null)
        {
            return Ok(office);
        }

        return NotFound();
    }

    /// <summary>Creates the office with the provided data.</summary>
    /// <param name="office">The office-related data. Should be sent in a request body.</param>
    /// <returns>The if of the created office. Ids are represented in the GUID (UUID) format.</returns>
    ///<response code="200">Success. Returns the id of the created office in the GUID (UUID) format.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    public async Task<IActionResult> CreateOffice([FromBody] OfficeDto office)
    {
        return Ok((await _officesRepository.CreateOfficeAsync(office)).ToString());
    }

    /// <summary>Updates the office data.</summary>
    /// <param name="id">The office ID. Ids are represented in the GUID (UUID) format.</param>
    /// <param name="officeUpdateData"> The updated office-related data. Should be sent in a request body.</param>
    /// <returns>Successful status code (200) if update succeeds.</returns>
    /// <response code="200">Success. The office update succeeded.</response>
    /// <response code="404">Failure. The office with the provided id is not found.</response>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(void))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(void))]
    public async Task<IActionResult> UpdateOffice([FromRoute] Guid id, [FromBody] OfficeDto officeUpdateData)
    {
        var office = await _officesRepository.GetOfficeAsync(id);
        if (office != null)
        {
            await _officesRepository.UpdateOfficeAsync(office, officeUpdateData);
            return Ok(office);
        }

        return NotFound();
    }

    /// <summary>Deletes the office from the system.</summary>
    /// <param name="id">The office ID. Ids are represented in the GUID (UUID) format.</param>
    /// <returns>Successful status code (204) if deletion succeeds.</returns>
    /// <response code="204">Success. The office has been deleted.</response>
    /// <response code="404">Failure. The office with the provided id is not found.</response>
    [HttpDelete("{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(void))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteOffice([FromRoute] Guid id)
    {
        var office = await _officesRepository.GetOfficeAsync(id);
        if (office != null)
        {
            await _officesRepository.DeleteOfficeAsync(office);
            return NoContent();
        }

        return NotFound();
    }
}