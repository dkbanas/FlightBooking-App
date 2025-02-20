using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("[controller]")]
[ApiController]
public class ReservationController(IReservationService reservationService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateReservation([FromBody] ReservationDTO request)
    {
        var result = await reservationService.CreateReservation(request);
        if (result == null)
            return BadRequest("Reservation could not be created.");

        return CreatedAtAction(nameof(GetUserReservations), new { userId = request.UserId }, result);
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<List<ReservationResponseDTO>>> GetUserReservations(int userId)
    {
        var reservations = await reservationService.GetUserReservations(userId);
        return Ok(reservations);
    }

    [HttpDelete("{reservationId}")]
    public async Task<IActionResult> CancelReservation(long reservationId)
    {
        var success = await reservationService.CancelReservation(reservationId);
        if (!success)
            return NotFound("Reservation not found.");

        return NoContent();
    }
}