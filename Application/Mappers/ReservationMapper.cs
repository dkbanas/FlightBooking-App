using Application.DTOs;
using Domain.Entities;

namespace Application.Mappers;

// Reservation | Request -> Entity, Entity -> Response
public class ReservationMapper
{
    // Converts a ReservationDTO to a Reservation entity
    public static Reservation ToEntity(ReservationDTO dto, Flight flight, User user)
    {
        return new Reservation
        {
            Flight = flight,
            FlightId = flight.Id,
            User = user,
            UserId = user.Id,
            SeatNumbers = dto.SeatNumbers.ToList(),
            ReservationDate = DateTime.UtcNow
        };
    }

    // Converts a Reservation entity to a ReservationResponseDTO
    public static ReservationResponseDTO ToResponse(Reservation reservation)
    {
        if (reservation.Flight == null || reservation.User == null || reservation.Flight.DepartureLocation == null || reservation.Flight.ArrivalLocation == null)
        {
            throw new ArgumentNullException("One of the required properties is null.");
        }
        
        return new ReservationResponseDTO
        {
            Id = reservation.Id,
            FlightNumber = reservation.Flight.FlightNumber,
            UserEmail = reservation.User.Email,
            SeatNumbers = reservation.SeatNumbers,
            ReservationDate = reservation.ReservationDate,
            DepartureTime = reservation.Flight.DepartureTime,
            Price = reservation.Flight.Price,
                
            DepartureName = reservation.Flight.DepartureLocation.Name,
            DepartureCity = reservation.Flight.DepartureLocation.City,
            DepartureCountry = reservation.Flight.DepartureLocation.Country,

            ArrivalName = reservation.Flight.ArrivalLocation.Name,
            ArrivalCity = reservation.Flight.ArrivalLocation.City,
            ArrivalCountry = reservation.Flight.ArrivalLocation.Country
        };
    }

    // Converts a list of Reservation entities to a list of ReservationResponseDTOs
    public static List<ReservationResponseDTO> ToResponseList(IEnumerable<Reservation> reservations)
    {
        return reservations.Select(ToResponse).ToList();
    }
}