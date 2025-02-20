namespace Application.DTOs;
// Data Transfer Object (DTO) representing an airport request
public class AirportDTO
{
    public string Name { get; set; }
    public string Code { get; set; } // IATA Code
    public string City { get; set; }
    public string Country { get; set; }
    public string CityPhotoUrl { get; set; }
    public string Continent { get; set; }
}