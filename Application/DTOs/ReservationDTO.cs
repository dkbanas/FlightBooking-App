namespace Application.DTOs;

// Data Transfer Object (DTO) for a flight reservation request
public class ReservationDTO
{
    public int FlightId { get; set; }
    public int UserId { get; set; }
    public List<string> SeatNumbers { get; set; } // List of seat numbers being reserved
}