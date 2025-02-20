using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

// Represents a seat that has been occupied on a specific flight
public class OccupiedSeat
{
    public int Id { get; set; }
    public int FlightId { get; set; } // Foreign key referencing the flight
    [ForeignKey("FlightId")]
    public Flight Flight { get; set; } // Navigation property to the associated flight
    public string Seat { get; set; } // Seat number that has been occupied
}