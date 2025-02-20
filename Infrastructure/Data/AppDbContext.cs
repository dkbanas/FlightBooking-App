using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

/// <summary>
/// Represents the application database context for managing entities in the database.
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Gets or sets the users in the database.
    /// </summary>
    public DbSet<User> users { get; set; }
    
    /// <summary>
    /// Gets or sets the airports in the database.
    /// </summary>
    public DbSet<Airport> airports { get; set; }

    /// <summary>
    /// Gets or sets the flights in the database.
    /// </summary>
    public DbSet<Flight> flights { get; set; }
    
    /// <summary>
    /// Gets or sets the occupied seats in the database.
    /// </summary>
    public DbSet<OccupiedSeat> occupiedSeats { get; set; }
    
    /// <summary>
    /// Gets or sets the reservations in the database.
    /// </summary>
    public DbSet<Reservation> reservations { get; set; }

    
    /// <summary>
    /// Configures the model with the specified <see cref="ModelBuilder"/>.
    /// </summary>
    /// <param name="modelBuilder">The model builder used to configure the model.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configures the one-to-many relationship between Flight and OccupiedSeat.
        modelBuilder.Entity<Flight>()
            .HasMany(f => f.OccupiedSeats)
            .WithOne(o => o.Flight)
            .HasForeignKey(o => o.FlightId);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                {
                    property.SetColumnType("timestamp with time zone");
                }
            }
        }
        
        
    }
}