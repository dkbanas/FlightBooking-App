using Application.Interfaces;
using Infrastructure.Data;
using Infrastructure.Seeders;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection;

/// <summary>
/// Provides extension methods for registering application services in the dependency injection container.
/// </summary>
public static class ServiceContainer
{
    /// <summary>
    /// Adds infrastructure services to the specified <see cref="IServiceCollection"/> for dependency injection.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="configuration">The configuration used to set up services.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> with added infrastructure services.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var imageDir = Path.Combine(Directory.GetCurrentDirectory(), "IMG", "Airports");
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAirportService>(provider => new AirportService(provider.GetRequiredService<AppDbContext>(), imageDir));
        services.AddScoped<IFlightService>(provider => new FlightService(provider.GetRequiredService<AppDbContext>()));
        services.AddScoped<IReservationService>(provider => new ReservationService(provider.GetRequiredService<AppDbContext>()));
        services.AddScoped<AirportSeeder>();
        services.AddScoped<FlightSeeder>();
        return services;
    }
}