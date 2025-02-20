using System.Text.Json;
using Application.Utils;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Seeders;

/// <summary>
/// Seeds the database with flight data from a JSON file.
/// </summary>
public class FlightSeeder(AppDbContext context, ILogger<AirportSeeder> logger)
{
    
    /// <summary>
    /// Seeds the flight data asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SeedAsync()
    {
        if (context.flights.Any())
        {
            logger.LogInformation("Flights already seeded.");
            return;
        }

        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var solutionRoot = Path.Combine(baseDirectory, "..", "..", "..", "..");
        var jsonFilePath = Path.Combine(solutionRoot, "Infrastructure", "Data", "flights.json");
        if (!File.Exists(jsonFilePath))
        {
            logger.LogError("Flight JSON file not found: " + jsonFilePath);
            return;
        }

        var jsonData = await File.ReadAllTextAsync(jsonFilePath);
        var flights = JsonSerializer.Deserialize<List<Flight>>(jsonData);

        if (flights == null || !flights.Any())
        {
            logger.LogError("No flights found in the JSON file.");
            return;
        }
        foreach (var flight in flights)
        {
            flight.Duration = DurationCalculator.CalculateDuration(flight.DepartureTime, flight.ArrivalTime);
        }

        logger.LogInformation("Deserialized flights: " + JsonSerializer.Serialize(flights));
        context.flights.AddRange(flights);
        await context.SaveChangesAsync();

        logger.LogInformation("Successfully seeded flights.");
    }
}