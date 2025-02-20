using Application.DTOs;
using Shared.Pagination;

namespace Application.Interfaces;

/// <summary>
/// Interface for airport-related operations.
/// </summary>
public interface IAirportService
{
    /// <summary>
    /// Retrieves a list of all airports.
    /// </summary>
    /// <returns>A list of <see cref="AirportResponseDTO"/> containing airport details.</returns>
    Task<List<AirportResponseDTO>> GetAllAirports();
    
    /// <summary>
    /// Searches for airports based on the specified query.
    /// </summary>
    /// <param name="query">The search query for airport names or codes.</param>
    /// <returns>A list of <see cref="AirportResponseDTO"/> matching the search criteria.</returns>
    Task<List<AirportResponseDTO>> SearchAirports(string query);

    /// <summary>
    /// Retrieves a paginated and sorted list of airports.
    /// </summary>
    /// <param name="query">The query parameters for pagination and sorting.</param>
    /// <param name="cancellationToken">The cancellation token for the operation.</param>
    /// <returns>A paginated list of <see cref="AirportResponseDTO"/>.</returns>
    Task<PagedList<AirportResponseDTO>> GetAirportsPaginatedAndSorted(AirportQuery query,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves an airport by its code.
    /// </summary>
    /// <param name="code">The airport code.</param>
    /// <returns>The <see cref="AirportResponseDTO"/> for the specified airport code.</returns>
    Task<AirportResponseDTO> GetAirportByCode(string code);
    
    /// <summary>
    /// Creates a new airport.
    /// </summary>
    /// <param name="request">The details of the airport to create.</param>
    /// <returns>The created <see cref="AirportResponseDTO"/>.</returns>
    Task<AirportResponseDTO> CreateAirport(AirportDTO request);
    
    /// <summary>
    /// Updates an existing airport by its code.
    /// </summary>
    /// <param name="code">The airport code to update.</param>
    /// <param name="request">The updated details of the airport.</param>
    /// <returns>The updated <see cref="AirportResponseDTO"/>.</returns>
    Task<AirportResponseDTO> UpdateAirport(string code, AirportDTO request);
    
    /// <summary>
    /// Deletes an airport by its IATA code.
    /// </summary>
    /// <param name="code">The airport code to delete.</param>
    /// <returns>True if the airport was deleted successfully; otherwise, false.</returns>
    Task<bool> DeleteAirport(string code);

    /// <summary>
    /// Uploads an image for a specified airport.
    /// </summary>
    /// <param name="fileData">The byte array of the image data.</param>
    /// <param name="fileName">The name of the file being uploaded.</param>
    /// <param name="airportCode">The code of the airport the image is associated with.</param>
    /// <returns>A string representing the upload result or path.</returns>
    String UploadAirportImage(byte[] fileData, string fileName, string airportCode);
}