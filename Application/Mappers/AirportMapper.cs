using Application.DTOs;
using Domain.Entities;

namespace Application.Mappers;

// Airport | Request -> Entity, Entity -> Response
public class AirportMapper
{
    // Converts an AirportDTO to an Airport entity
    public static Airport ToEntity(AirportDTO airportDTO)
    {
        return new Airport
        {
            Name = airportDTO.Name,
            Code = airportDTO.Code,
            City = airportDTO.City,
            Country = airportDTO.Country,
            CreatedAt = DateTime.UtcNow,
            CityPhotoUrl = airportDTO.CityPhotoUrl,
            Continent = airportDTO.Continent,
        };
    }
    // Converts an Airport entity to an AirportResponseDTO
    public static AirportResponseDTO ToResponse(Airport airport)
    {
        return new AirportResponseDTO
        {
            Id = airport.Id,
            Name = airport.Name,
            Code = airport.Code,
            City = airport.City,
            Country = airport.Country,
            CreatedAt = airport.CreatedAt,
            CityPhotoUrl = airport.CityPhotoUrl,
            Continent = airport.Continent
        };
    }
    
    // Converts a list of Airport entities to a list of AirportResponseDTOs
    public static List<AirportResponseDTO> ToResponseList(List<Airport> airports)
    {
        return airports.Select(ToResponse).ToList();
    }
}