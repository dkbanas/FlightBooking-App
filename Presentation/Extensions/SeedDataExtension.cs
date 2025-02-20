using Infrastructure.Seeders;

namespace Presentation.Extensions;

public static class SeedDataExtension
{
    public static async Task RunSeedDataAsync(this IServiceProvider services)
    {
        using (var scope = services.CreateScope())
        {
            var airportSeeder = scope.ServiceProvider.GetRequiredService<AirportSeeder>();
            await airportSeeder.SeedAsync();
            
            var flightSeeder = scope.ServiceProvider.GetRequiredService<FlightSeeder>();
            await flightSeeder.SeedAsync();
        }
    }
}