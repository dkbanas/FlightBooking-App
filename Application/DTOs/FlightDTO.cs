using System.ComponentModel.DataAnnotations;
namespace Application.DTOs;
// Data Transfer Object (DTO) for flight request
public class FlightDTO
{
    [Required]
    public string FlightNumber { get; set; } //IATA Code

    [Required]
    public int DepartureLocationId { get; set; }

    [Required]
    public int ArrivalLocationId { get; set; }

    [Required]
    public DateTime DepartureTime { get; set; }

    [Required]
    public DateTime ArrivalTime { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public string Airline { get; set; }
}