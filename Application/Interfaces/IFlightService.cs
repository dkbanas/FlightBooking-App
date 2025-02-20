using Application.DTOs;
using Shared.Pagination;

namespace Application.Interfaces;

/// <summary>
/// Interface for flight-related operations.
/// </summary>
public interface IFlightService
{
    
    /// <summary>
    /// Retrieves a list of all flights.
    /// </summary>
    /// <returns>A list of <see cref="FlightResponseDTO"/>.</returns>
    Task<List<FlightResponseDTO>> GetAllFlightsAsync();

    /// <summary>
    /// Retrieves a paginated and sorted list of flights.
    /// </summary>
    /// <param name="query">The query parameters for pagination and sorting.</param>
    /// <param name="cancellationToken">The cancellation token for the operation.</param>
    /// <returns>A paginated list of <see cref="FlightResponseDTO"/>.</returns>
    Task<PagedList<FlightResponseDTO>> GetFlightsPaginatedAndSortedAsync(FlightQuery query, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a flight by its flight number.
    /// </summary>
    /// <param name="flightNumber">The flight number.</param>
    /// <returns>The <see cref="FlightResponseDTO"/> for the specified flight number.</returns>
    Task<FlightResponseDTO> GetFlightByFlightNumberAsync(string flightNumber);

    /// <summary>
    /// Creates a new flight.
    /// </summary>
    /// <param name="flightDTO">The flight details for creation.</param>
    /// <returns>The created <see cref="FlightResponseDTO"/>.</returns>
    Task<FlightResponseDTO> CreateFlightAsync(FlightDTO flightDTO);

    /// <summary>
    /// Deletes a flight by its flight number.
    /// </summary>
    /// <param name="flightNumber">The flight number to delete.</param>
    /// <returns>True if the flight was deleted successfully; otherwise, false.</returns>
    Task<bool> DeleteFlightAsync(string flightNumber);

    /// <summary>
    /// Searches for flights based on the specified criteria.
    /// </summary>
    /// <param name="departureLocationId">The ID of the departure location.</param>
    /// <param name="arrivalLocationId">The ID of the arrival location.</param>
    /// <param name="departureDate">The scheduled departure date.</param>
    /// <param name="returnDate">The optional return date for round trips.</param>
    /// <param name="numberOfPassengers">The number of passengers traveling.</param>
    /// <param name="roundTrip">Indicates whether the search is for a round trip.</param>
    /// <returns>A list of <see cref="RoundTripFlightResponseDTO"/> matching the search criteria.</returns>
    Task<List<RoundTripFlightResponseDTO>> SearchFlightsAsync(int departureLocationId, int arrivalLocationId, DateTime departureDate, DateTime? returnDate, int numberOfPassengers, bool roundTrip);

    /// <summary>
    /// Retrieves the top 5 cheapest flights.
    /// </summary>
    /// <returns>A list of the top 5 <see cref="FlightResponseDTO"/>.</returns>
    Task<List<FlightResponseDTO>> GetTop5CheapestFlightsAsync();
}