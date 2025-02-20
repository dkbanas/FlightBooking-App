namespace Application.DTOs;
// Response DTO for returning flight details
public class FlightResponseDTO
{
    public int Id { get; set; }
    public string FlightNumber { get; set; }
    public AirportResponseDTO DepartureLocation { get; set; }
    public AirportResponseDTO ArrivalLocation { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public decimal Price { get; set; }
    public int TotalSeats { get; set; }
    public string Duration { get; set; }
    public string Airline { get; set; }
    public int AvailableSeats { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<string> AvailableSeatsList { get; set; }
}