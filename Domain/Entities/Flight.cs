using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;
// Represents a flight with scheduling and pricing details
public class Flight
{
    public int Id { get; set; }
    public string FlightNumber { get; set; }
    
    public int DepartureLocationId { get; set; } // Foreign key to departure airport
    [ForeignKey("DepartureLocationId")]
    public Airport DepartureLocation { get; set; } // Navigation property for departure location
    
    public int ArrivalLocationId { get; set; } // Foreign key to arrival airport
    [ForeignKey("ArrivalLocationId")]
    public Airport ArrivalLocation { get; set; } // Navigation property for arrival location
    
    public DateTime DepartureTime { get; set; } // Scheduled departure time
    public DateTime ArrivalTime { get; set; } // Scheduled arrival time
    public decimal Price { get; set; } // Ticket price
    public int TotalSeats { get; set; } = 120; // Total seats available on the flight
    public int AvailableSeats { get; set; } = 120; // Remaining available seats
    public string Duration { get; set; } // Flight duration
    public string Airline { get; set; } // Airline operating the flight
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    
    public HashSet<OccupiedSeat> OccupiedSeats { get; set; } = new HashSet<OccupiedSeat>(); // Collection of occupied seats on the flight
    
    /// <summary>
    /// Reserves a list of seats if they are available.
    /// </summary>
    /// <param name="seatNumbers">List of seat numbers to reserve</param>
    /// <exception cref="Exception">Thrown if any of the requested seats are already taken</exception>
    public void ReserveSeats(IEnumerable<string> seatNumbers)
    {
        var seatList = seatNumbers.ToList();

        if (!IsSeatsAvailable(seatList))
        {
            throw new Exception("Some selected seats are already taken.");
        }

        foreach (var seat in seatList)
        {
            OccupiedSeats.Add(new OccupiedSeat { FlightId = Id, Seat = seat });
        }
        AvailableSeats -= seatList.Count;
    }

    
    /// <summary>
    /// Releases the specified seats, making them available for booking again.
    /// </summary>
    /// <param name="seatNumbers">List of seat numbers to release</param>
    public void ReleaseSeats(IEnumerable<string> seatNumbers)
    {
        var seatsToRemove = OccupiedSeats.Where(o => seatNumbers.Contains(o.Seat)).ToList();

        foreach (var seat in seatsToRemove)
        {
            OccupiedSeats.Remove(seat);
        }

        AvailableSeats += seatNumbers.Count();
    }

    /// <summary>
    /// Checks if the requested seats are available for reservation.
    /// </summary>
    /// <param name="seatNumbers">List of seat numbers to check</param>
    /// <returns>True if all seats are available; otherwise, false</returns>
    public bool IsSeatsAvailable(IEnumerable<string> seatNumbers)
    {
        return seatNumbers.All(seat => !string.IsNullOrEmpty(seat) && !OccupiedSeats.Any(o => o.Seat == seat));
    }
}