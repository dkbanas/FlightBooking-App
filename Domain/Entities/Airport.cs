namespace Domain.Entities;
// Represents an airport with location details
public class Airport
{
    public int Id {get; set; }
    public string Name {get; set; } = string.Empty;
    public string Code {get; set; } = string.Empty; // Airport code (e.g., JFK, LAX)
    public string City {get; set; } = string.Empty; // City where the airport is located
    public string Country {get; set; } = string.Empty; // Country where the airport is located
    public string CityPhotoUrl {get; set; } = string.Empty; // URL of the city's representative photo
    public string Continent {get; set; } = string.Empty; // Continent where the airport is situated
    public DateTime CreatedAt {get; set; } = DateTime.UtcNow;
}