namespace Domain.Entities;

// Represents a flight reservation made by a user
public class Reservation
{
    public int Id {get; set; }
    
    public int UserId { get; set; } // Foreign key referencing the user
    public User User { get; set; } // Navigation property to the user who made the reservation
    
    public int FlightId { get; set; } // Foreign key referencing the flight
    public Flight Flight { get; set; } // Navigation property to the associated flight
    
    public List<string> SeatNumbers { get; set; } = new List<string>(); // List of seat numbers reserved by the user
    
    public DateTime ReservationDate { get; set; } = DateTime.UtcNow;
}