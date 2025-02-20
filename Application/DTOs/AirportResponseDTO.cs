namespace Application.DTOs;

// Response DTO for returning airport details
public class AirportResponseDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; } // IATA Code
    public string City { get; set; }
    public string Country { get; set; }
    public string CityPhotoUrl { get; set; }
    public string Continent { get; set; }
    public DateTime CreatedAt { get; set; }
}