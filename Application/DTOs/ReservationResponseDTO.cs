namespace Application.DTOs;
// Response DTO for returning reservation details
public class ReservationResponseDTO
{
    public int Id { get; set; }
    public string FlightNumber { get; set; }
    public string UserEmail { get; set; }
    public List<string> SeatNumbers { get; set; }
    public DateTime ReservationDate { get; set; }

    public string DepartureName { get; set; }
    public string DepartureCity { get; set; }
    public string DepartureCountry { get; set; }
    
    public string ArrivalName { get; set; }
    public string ArrivalCity { get; set; }
    public string ArrivalCountry { get; set; }
    
    public DateTime DepartureTime { get; set; }
    public decimal Price { get; set; }
}