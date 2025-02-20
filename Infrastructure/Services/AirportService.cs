using Application.DTOs;
using Application.Interfaces;
using Application.Mappers;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Pagination;
using Shared.Sorting;

namespace Infrastructure.Services;

/// <summary>
/// Service class for managing airports.
/// </summary>
public class AirportService(AppDbContext context,string imageDir) : IAirportService
{
    /// <summary>
    /// Retrieves all airports from the database.
    /// </summary>
    /// <returns>A list of <see cref="AirportResponseDTO"/> representing all airports.</returns>
    public async Task<List<AirportResponseDTO>> GetAllAirports()
    {
        var result = await context.airports.ToListAsync();
        return AirportMapper.ToResponseList(result);
    }
    
    /// <summary>
    /// Searches for airports based on a query string in their name, city, country, or code.
    /// </summary>
    /// <param name="query">The search query to filter airports.</param>
    /// <returns>A list of <see cref="AirportResponseDTO"/> matching the search criteria.</returns>
    public async Task<List<AirportResponseDTO>> SearchAirports(string query)
    {
        query = query.ToLower();
        
        var airports = 
            await context.airports.Where(a => 
                a.Name.ToLower().Contains(query) || 
                a.City.ToLower().Contains(query) || 
                a.Country.ToLower().Contains(query) || 
                a.Code.ToLower().Contains(query)
            ).ToListAsync();

        return AirportMapper.ToResponseList(airports);
    }
    
    /// <summary>
    /// Retrieves airports with pagination and sorting based on the specified query parameters.
    /// </summary>
    /// <param name="query">The query containing pagination and sorting parameters.</param>
    /// <param name="cancellationToken">The cancellation token for the operation.</param>
    /// <returns>A <see cref="PagedList{AirportResponseDTO}"/> containing the paginated and sorted list of airports.</returns>
    public async Task<PagedList<AirportResponseDTO>> GetAirportsPaginatedAndSorted(AirportQuery query, CancellationToken cancellationToken = default)
    {
        IQueryable<Airport> airportQuery = context.airports.AsQueryable();

        // Filter by continent if provided
        if (!string.IsNullOrEmpty(query.Continent))
        {
            airportQuery = airportQuery.Where(a => a.Continent == query.Continent);
        }

        // Apply sorting if specified
        if (!string.IsNullOrEmpty(query.SortColumn))
        {
            bool descending = query.SortOrder == SortOrder.Descending;
            airportQuery = airportQuery.OrderByDynamic(query.SortColumn, descending);
        }

        // Apply pagination
        var paginatedList = await PagedListExtensions<Airport>.CreateAsync(airportQuery, query.Page, query.PageSize, cancellationToken);

        // Convert to DTOs
        var responseDTOs = paginatedList.Items.Select(airport => AirportMapper.ToResponse(airport)).ToList();

        return new PagedList<AirportResponseDTO>(responseDTOs, query.Page, query.PageSize, paginatedList.TotalCount);
    }


    /// <summary>
    /// Retrieves an airport by its code.
    /// </summary>
    /// <param name="code">The airport code to search for.</param>
    /// <returns>The <see cref="AirportResponseDTO"/> for the specified airport, or null if not found.</returns>
    public async Task<AirportResponseDTO> GetAirportByCode(string code)
    {
        var result = await context.airports.FirstOrDefaultAsync(airport => airport.Code == code);
        return result != null ? AirportMapper.ToResponse(result) : null;
    }

    /// <summary>
    /// Creates a new airport based on the provided details.
    /// </summary>
    /// <param name="request">The <see cref="AirportDTO"/> containing airport details.</param>
    /// <returns>The created <see cref="AirportResponseDTO"/> or null if the airport code already exists.</returns>
    public async Task<AirportResponseDTO> CreateAirport(AirportDTO request)
    {
        if (await context.airports.AnyAsync(a => a.Code == request.Code))
            return null;

        var result = AirportMapper.ToEntity(request);
        context.airports.Add(result);
        await context.SaveChangesAsync();

        return AirportMapper.ToResponse(result);
    }

    /// <summary>
    /// Updates an existing airport's details.
    /// </summary>
    /// <param name="code">The code of the airport to update.</param>
    /// <param name="request">The <see cref="AirportDTO"/> containing updated airport details.</param>
    /// <returns>The updated <see cref="AirportResponseDTO"/> or null if the airport was not found.</returns>
    public async Task<AirportResponseDTO> UpdateAirport(string code, AirportDTO request)
    {
        var airport = await context.airports.FirstOrDefaultAsync(a => a.Code == code);
        if (airport == null)
            return null;

        airport.Name = request.Name;
        airport.City = request.City;
        airport.Country = request.Country;
        airport.CityPhotoUrl = request.CityPhotoUrl;
        airport.Continent = request.Continent;

        context.airports.Update(airport);
        await context.SaveChangesAsync();

        return AirportMapper.ToResponse(airport);
    }

    /// <summary>
    /// Deletes an airport by its code.
    /// </summary>
    /// <param name="code">The code of the airport to delete.</param>
    /// <returns>True if the airport was successfully deleted; otherwise, false.</returns>
    public async Task<bool> DeleteAirport(string code)
    {
        var result = await context.airports.FirstOrDefaultAsync(a => a.Code == code);
        if (result == null)
            return false;

        context.airports.Remove(result);
        await context.SaveChangesAsync();
        return true;
    }
    
    /// <summary>
    /// Uploads an image for an airport.
    /// </summary>
    /// <param name="fileData">The byte array representing the image file.</param>
    /// <param name="fileName">The name of the file being uploaded.</param>
    /// <param name="airportCode">The code of the airport to associate the image with.</param>
    /// <returns>The URL of the uploaded image.</returns>
    /// <exception cref="ArgumentException">Thrown when the file is empty or invalid.</exception>
    public string UploadAirportImage(byte[] fileData, string fileName, string airportCode)
    {
        if (fileData == null || fileData.Length == 0)
        {
            throw new ArgumentException("File is empty");
        }

        try
        {
            // Get the file extension from the file name
            string fileExtension = GetFileExtension(fileName);

            // Create a new file name using the airport code
            string newFileName = $"{airportCode}.{fileExtension}";

            // Combine the directory path with the new file name
            string filePath = Path.Combine(imageDir, newFileName);

            // Save the file to the target location
            File.WriteAllBytes(filePath, fileData);

            // Return the relative URL of the uploaded image
            return $"http://localhost:5111/IMG/Airports/{newFileName}";
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to upload image", ex);
        }
    }

    /// <summary>
    /// Gets the file extension from the provided file name.
    /// </summary>
    /// <param name="fileName">The name of the file.</param>
    /// <returns>The file extension as a string.</returns>
    /// <exception cref="ArgumentException">Thrown when the file name is invalid.</exception>
    private string GetFileExtension(string fileName)
    {
        if (string.IsNullOrEmpty(fileName) || !fileName.Contains('.'))
        {
            throw new ArgumentException("Invalid file name");
        }

        return fileName.Split('.').Last();
    }
    
}