using Application.DTOs;
using Application.Interfaces;
using Application.Mappers;
using Application.Utils;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Pagination;
using Shared.Sorting;

namespace Infrastructure.Services;

/// <summary>
/// Service class for managing flights.
/// </summary>
public class FlightService(AppDbContext context) : IFlightService
{
    /// <summary>
    /// Retrieves all flights from the database.
    /// </summary>
    /// <returns>A list of <see cref="FlightResponseDTO"/> representing all flights.</returns>
    public async Task<List<FlightResponseDTO>> GetAllFlightsAsync()
    {
        var result = await context.flights
            .Include(f => f.DepartureLocation)
            .Include(f => f.ArrivalLocation)
            .ToListAsync();
        return FlightMapper.ToResponseList(result);
    }

    /// <summary>
    /// Retrieves flights with pagination and sorting based on the specified query parameters.
    /// </summary>
    /// <param name="query">The query containing pagination and sorting parameters.</param>
    /// <param name="cancellationToken">The cancellation token for the operation.</param>
    /// <returns>A <see cref="PagedList{FlightResponseDTO}"/> containing the paginated and sorted list of flights.</returns>
    public async Task<PagedList<FlightResponseDTO>> GetFlightsPaginatedAndSortedAsync(FlightQuery query, CancellationToken cancellationToken = default)
    {
        IQueryable<Flight> flightQuery = context.flights
            .Include(f => f.DepartureLocation)  
            .Include(f => f.ArrivalLocation)    
            .Include(f => f.OccupiedSeats) 
            .AsQueryable();

        // Apply sorting
        if (!string.IsNullOrEmpty(query.SortColumn))
        {
            bool descending = query.SortOrder == SortOrder.Descending;
            flightQuery = flightQuery.OrderByDynamic(query.SortColumn, descending);
        }

        // Apply pagination
        var paginatedList = await PagedListExtensions<Flight>.CreateAsync(flightQuery, query.Page, query.PageSize, cancellationToken);
        var responseDTOs = paginatedList.Items.Select(FlightMapper.ToResponse).ToList();
    
        return new PagedList<FlightResponseDTO>(responseDTOs, query.Page, query.PageSize, paginatedList.TotalCount);
    }

    /// <summary>
    /// Retrieves a flight by its flight number.
    /// </summary>
    /// <param name="flightNumber">The flight number to search for.</param>
    /// <returns>The <see cref="FlightResponseDTO"/> for the specified flight, or null if not found.</returns>
    public async Task<FlightResponseDTO> GetFlightByFlightNumberAsync(string flightNumber)
    {
        var result = await context.flights
            .Include(f => f.DepartureLocation)   
            .Include(f => f.ArrivalLocation)     
            .Include(f => f.OccupiedSeats)       
            .FirstOrDefaultAsync(f => f.FlightNumber == flightNumber);
    
        return result != null ? FlightMapper.ToResponse(result) : null;
    }

    /// <summary>
    /// Creates a new flight based on the provided details.
    /// </summary>
    /// <param name="request">The <see cref="FlightDTO"/> containing flight details.</param>
    /// <returns>The created <see cref="FlightResponseDTO"/>.</returns>
    public async Task<FlightResponseDTO> CreateFlightAsync(FlightDTO request)
    {
        var departureLocation = await context.airports.FindAsync(request.DepartureLocationId);
        var arrivalLocation = await context.airports.FindAsync(request.ArrivalLocationId);
        
        var flight = FlightMapper.ToEntity(request, departureLocation, arrivalLocation);
        
        context.flights.Add(flight);
        await context.SaveChangesAsync();
        return FlightMapper.ToResponse(flight);
    }

    /// <summary>
    /// Deletes a flight by its flight number.
    /// </summary>
    /// <param name="flightNumber">The flight number of the flight to delete.</param>
    /// <returns>True if the flight was successfully deleted; otherwise, false.</returns>
    public async Task<bool> DeleteFlightAsync(string flightNumber)
    {
        var flight = await context.flights.FirstOrDefaultAsync(f => f.FlightNumber == flightNumber);
        if (flight == null) return false;
        
        context.flights.Remove(flight);
        await context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Searches for flights based on departure and arrival locations, dates, and passenger count.
    /// </summary>
    /// <param name="departureLocationId">The ID of the departure location.</param>
    /// <param name="arrivalLocationId">The ID of the arrival location.</param>
    /// <param name="departureDate">The date of departure.</param>
    /// <param name="returnDate">The date of return (if round trip).</param>
    /// <param name="numberOfPassengers">The number of passengers.</param>
    /// <param name="roundTrip">Indicates whether the search is for a round trip.</param>
    /// <returns>A list of <see cref="RoundTripFlightResponseDTO"/> containing the found flights.</returns>
    /// <exception cref="ArgumentNullException">Thrown when return date is required for round trips but not provided.</exception>
    public async Task<List<RoundTripFlightResponseDTO>> SearchFlightsAsync(int departureLocationId, int arrivalLocationId, DateTime departureDate, DateTime? returnDate,
        int numberOfPassengers, bool roundTrip)
    {
        // Define the departure date range
        DateTime departureStart = departureDate.Date;
        DateTime departureEnd = departureDate.Date.AddDays(1).AddTicks(-1);

        // Get outbound flights
        var outboundFlights = await FindFlightsAsync(departureLocationId, arrivalLocationId, departureStart, departureEnd, numberOfPassengers);

        // If it's not a round trip, return only the outbound flights
        if (!roundTrip)
        {
            return outboundFlights.Select(dto => new RoundTripFlightResponseDTO(dto, null)).ToList();
        }

        if (!returnDate.HasValue)
        {
            throw new ArgumentNullException(nameof(returnDate), "Return date is required for round trips.");
        }

        // Define the return date range
        DateTime returnStart = returnDate.Value.Date;
        DateTime returnEnd = returnDate.Value.Date.AddDays(1).AddTicks(-1);

        // Get return flights
        var returnFlights = await FindFlightsAsync(arrivalLocationId, departureLocationId, returnStart, returnEnd, numberOfPassengers);

        // Combine outbound and return flights into round trip responses
        return outboundFlights.SelectMany(outbound => returnFlights.Select(returnFlight =>
            new RoundTripFlightResponseDTO(outbound, returnFlight))).ToList();
    }
    
    /// <summary>
    /// Finds flights based on the specified parameters.
    /// </summary>
    /// <param name="departureLocationId">The ID of the departure location.</param>
    /// <param name="arrivalLocationId">The ID of the arrival location.</param>
    /// <param name="start">The start date and time for departure.</param>
    /// <param name="end">The end date and time for departure.</param>
    /// <param name="numberOfPassengers">The number of passengers.</param>
    /// <returns>A list of available <see cref="FlightResponseDTO"/>.</returns>
    private async Task<List<FlightResponseDTO>> FindFlightsAsync(int departureLocationId, int arrivalLocationId,
        DateTime start, DateTime end, int numberOfPassengers)
    {
        var availableFlights = await context.flights
            .Include(f => f.DepartureLocation)
            .Include(f => f.ArrivalLocation)
            .Include(f => f.OccupiedSeats) 
            .Where(f => f.DepartureLocationId == departureLocationId && f.ArrivalLocationId == arrivalLocationId
                                                                     && f.DepartureTime >= start && f.DepartureTime <= end
                                                                     && f.AvailableSeats >= numberOfPassengers)
            .ToListAsync();

        return availableFlights.Select(MapFlightToResponseDTO).ToList();
    }
    
    /// <summary>
    /// Maps a flight entity to a <see cref="FlightResponseDTO"/>.
    /// </summary>
    /// <param name="flight">The flight entity to map.</param>
    /// <returns>The mapped <see cref="FlightResponseDTO"/>.</returns>
    private FlightResponseDTO MapFlightToResponseDTO(Flight flight)
    {
        var dto = FlightMapper.ToResponse(flight);
        dto.AvailableSeatsList = GetAvailableSeats(flight.TotalSeats, flight.OccupiedSeats);
        return dto;
    }
    
    /// <summary>
    /// Retrieves a list of available seats for a flight.
    /// </summary>
    /// <param name="totalSeats">The total number of seats in the flight.</param>
    /// <param name="occupiedSeats">The set of occupied seats.</param>
    /// <returns>A list of available seat numbers as strings.</returns>
    private List<string> GetAvailableSeats(int totalSeats, HashSet<OccupiedSeat> occupiedSeats)
    {
        var occupiedSeatNumbers = occupiedSeats.Select(s => s.Seat).ToHashSet();  // Extract seat numbers from OccupiedSeat entities

        return Enumerable.Range(1, totalSeats)
            .Select(i => i.ToString())
            .Where(seat => !occupiedSeatNumbers.Contains(seat))
            .ToList();
    }

    /// <summary>
    /// Retrieves the top 5 cheapest flights from the database.
    /// </summary>
    /// <returns>A list of the top 5 cheapest <see cref="FlightResponseDTO"/>.</returns>
    public async Task<List<FlightResponseDTO>> GetTop5CheapestFlightsAsync()
    {
        var flights = await context.flights
            .Include(f => f.DepartureLocation)   
            .Include(f => f.ArrivalLocation)     
            .OrderBy(f => f.Price)
            .Take(5)
            .ToListAsync();
    
        return FlightMapper.ToResponseList(flights);
    }
}