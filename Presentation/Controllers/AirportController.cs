using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Pagination;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class AirportController(IAirportService airportService) : ControllerBase
{
    [HttpGet] 
    public async Task<ActionResult<List<AirportResponseDTO>>> GetAll()
    {
        return Ok(await airportService.GetAllAirports());
    }
    
    [HttpGet("paginated-sorted")]
    public async Task<ActionResult<PagedList<AirportResponseDTO>>> GetAirports([FromQuery] AirportQuery query)
    {
        var result = await airportService.GetAirportsPaginatedAndSorted(query);
        return Ok(result);
    }
    
    [HttpGet("search")]
    public async Task<ActionResult<List<AirportResponseDTO>>> SearchAirports(string query)
    {
        var result = await airportService.SearchAirports(query);
        return Ok(result);
    }
    
    [HttpGet("{code}")]
    public async Task<ActionResult<AirportResponseDTO>> GetByCode(string code)
    {
        var airport = await airportService.GetAirportByCode(code);
        if (airport == null)
            return NotFound($"Airport with code {code} not found");

        return Ok(airport);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<AirportResponseDTO>> Create(AirportDTO request)
    {
        var createdAirport = await airportService.CreateAirport(request);
        if (createdAirport == null)
            return BadRequest($"Airport with code {request.Code} already exists");

        return Ok(createdAirport);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{code}")]
    public async Task<ActionResult<AirportResponseDTO>> Update(string code, AirportDTO request)
    {
        var updatedAirport = await airportService.UpdateAirport(code, request);
        if (updatedAirport == null)
            return NotFound($"Airport with code {code} not found");

        return Ok(updatedAirport);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{code}")]
    public async Task<IActionResult> Delete(string code)
    {
        var deleted = await airportService.DeleteAirport(code);
        if (!deleted)
            return NotFound($"Airport with code {code} not found");

        return NoContent();
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost("upload-image")]
    public IActionResult UploadImage(IFormFile file,string airportCode)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }
        

        using (var memoryStream = new MemoryStream())
        {
            file.CopyTo(memoryStream);
            var fileData = memoryStream.ToArray();
            
            var fileUrl = airportService.UploadAirportImage(fileData, file.FileName, airportCode);

            return Ok(new { ImageUrl = fileUrl });
        }
    }
}