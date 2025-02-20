using Application.DTOs;
using Application.Utils;
using Domain.Entities;

namespace Application.Mappers;

// Flight | Request -> Entity, Entity -> Response
public class FlightMapper
{
    // Converts a FlightDTO to a Flight entity, requiring valid departure and arrival locations
    public static Flight ToEntity(FlightDTO flightDTO, Airport departureLocation, Airport arrivalLocation)
    {
        if (departureLocation == null || arrivalLocation == null)
        {
            throw new ArgumentException("Invalid departure or arrival airport");
        }

        return new Flight
        {
            FlightNumber = flightDTO.FlightNumber,
            DepartureTime = DateTime.SpecifyKind(flightDTO.DepartureTime, DateTimeKind.Utc),
            ArrivalTime = DateTime.SpecifyKind(flightDTO.ArrivalTime, DateTimeKind.Utc),
            Price = flightDTO.Price,
            Airline = flightDTO.Airline,
            DepartureLocation = departureLocation,
            ArrivalLocation = arrivalLocation,
            CreatedAt = DateTime.UtcNow,
            Duration = DurationCalculator.CalculateDuration(flightDTO.DepartureTime, flightDTO.ArrivalTime)
        };
        
    }
    // Converts a Flight entity to a FlightResponseDTO
    public static FlightResponseDTO ToResponse(Flight flight)
    {
        
        var availableSeatsList = flight.OccupiedSeats?.Select(s => s.Seat).ToList() ?? new List<string>();
        
        return new FlightResponseDTO
        {
            Id = flight.Id,
            FlightNumber = flight.FlightNumber,
            DepartureLocation = new AirportResponseDTO
            {
                Id = flight.DepartureLocation.Id,
                Name = flight.DepartureLocation.Name,
                Code = flight.DepartureLocation.Code,
                City = flight.DepartureLocation.City,
                Country = flight.DepartureLocation.Country,
                CreatedAt = flight.DepartureLocation.CreatedAt,
                Continent = flight.DepartureLocation.Continent,
                CityPhotoUrl = flight.DepartureLocation.CityPhotoUrl
            },
            ArrivalLocation = new AirportResponseDTO
            {
                Id = flight.ArrivalLocation.Id,
                Name = flight.ArrivalLocation.Name,
                Code = flight.ArrivalLocation.Code,
                City = flight.ArrivalLocation.City,
                Country = flight.ArrivalLocation.Country,
                CreatedAt = flight.ArrivalLocation.CreatedAt,
                Continent = flight.ArrivalLocation.Continent,
                CityPhotoUrl = flight.ArrivalLocation.CityPhotoUrl
            },
            DepartureTime = DateTime.SpecifyKind(flight.DepartureTime, DateTimeKind.Utc),
            ArrivalTime = DateTime.SpecifyKind(flight.ArrivalTime, DateTimeKind.Utc),
            Price = flight.Price,
            TotalSeats = flight.TotalSeats,
            Duration = flight.Duration,
            Airline = flight.Airline,
            AvailableSeats = flight.AvailableSeats,
            CreatedAt = DateTime.SpecifyKind(flight.CreatedAt, DateTimeKind.Utc),
            AvailableSeatsList = availableSeatsList
        };
    }
    // Converts a list of Flight entities to a list of FlightResponseDTOs
    public static List<FlightResponseDTO> ToResponseList(List<Flight> flights)
    {
        return flights.Select(ToResponse).ToList();
    }
}