using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class FlightController(IFlightService flightService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllFlights()
    {
        var flights = await flightService.GetAllFlightsAsync();
        return Ok(flights);
    }
    
    [HttpGet("paginated-sorted")]
    public async Task<IActionResult> GetFlightsPaginatedAndSorted([FromQuery] FlightQuery query, CancellationToken cancellationToken = default)
    {
        // Validate the query parameters
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Call the service method to get the paginated and sorted flights
        var flights = await flightService.GetFlightsPaginatedAndSortedAsync(query, cancellationToken);

        // Return the result
        return Ok(flights);
    }
    
    [HttpGet("{flightNumber}")]
    public async Task<IActionResult> GetFlightByFlightNumber(string flightNumber)
    {
        var flight = await flightService.GetFlightByFlightNumberAsync(flightNumber);
        if (flight == null)
        {
            return NotFound("Flight not found.");
        }

        return Ok(flight);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateFlight([FromBody] FlightDTO flightDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdFlight = await flightService.CreateFlightAsync(flightDto);
        return CreatedAtAction(nameof(GetFlightByFlightNumber), new { flightNumber = createdFlight.FlightNumber }, createdFlight);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpDelete("{flightNumber}")]
    public async Task<IActionResult> DeleteFlight(string flightNumber)
    {
        var result = await flightService.DeleteFlightAsync(flightNumber);
        if (!result)
        {
            return NotFound("Flight not found.");
        }

        return NoContent();
    }
    
    [HttpGet("search")]
    public async Task<IActionResult> SearchFlights([FromQuery] int departureLocationId,
        [FromQuery] int arrivalLocationId,
        [FromQuery] DateTime departureDate,
        [FromQuery] DateTime? returnDate,
        [FromQuery] int numberOfPassengers,
        [FromQuery] bool roundTrip)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var flights = await flightService.SearchFlightsAsync(
            departureLocationId, 
            arrivalLocationId, 
            departureDate,
            returnDate, 
            numberOfPassengers, 
            roundTrip);

        return Ok(flights);
    }
    
    [HttpGet("top5-cheapest")]
    public async Task<IActionResult> GetTop5CheapestFlights()
    {
        var flights = await flightService.GetTop5CheapestFlightsAsync();
        return Ok(flights);
    }
    
}