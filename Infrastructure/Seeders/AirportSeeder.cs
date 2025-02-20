using System.Text.Json;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Seeders;

/// <summary>
/// Seeds the database with airport data from a JSON file.
/// </summary>
public class AirportSeeder(AppDbContext context, ILogger<AirportSeeder> logger)
{
    
    /// <summary>
    /// Seeds the airport data asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SeedAsync()
    {
        if (context.airports.Any())
        {
            logger.LogInformation("Airports already seeded.");
            return;
        }

        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var solutionRoot = Path.Combine(baseDirectory, "..", "..", "..", "..");
        var jsonFilePath = Path.Combine(solutionRoot, "Infrastructure", "Data", "airports.json");
        if (!File.Exists(jsonFilePath))
        {
            logger.LogError("Airport JSON file not found: " + jsonFilePath);
            return;
        }

        var jsonData = await File.ReadAllTextAsync(jsonFilePath);
        var airports = JsonSerializer.Deserialize<List<Airport>>(jsonData);

        if (airports == null || !airports.Any())
        {
            logger.LogError("No airports found in the JSON file.");
            return;
        }

        logger.LogInformation("Deserialized airports: " + JsonSerializer.Serialize(airports));
        context.airports.AddRange(airports);
        await context.SaveChangesAsync();

        logger.LogInformation("Successfully seeded airports.");
    }
}