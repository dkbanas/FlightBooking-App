using Application.DTOs;
using Application.Interfaces;
using Application.Mappers;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class ReservationService(AppDbContext context) : IReservationService
{
    public async Task<List<ReservationResponseDTO>> GetUserReservations(int userId)
        {
            var reservations = context.reservations
                .Include(r => r.Flight)
                .ThenInclude(f => f.DepartureLocation)
                .Include(r => r.Flight)
                .ThenInclude(f => f.ArrivalLocation)
                .Include(r => r.User)
                .Where(r => r.UserId == userId)
                .ToList();
            
            return reservations.Select(ReservationMapper.ToResponse).ToList();
        }

        public async Task<ReservationResponseDTO> CreateReservation(ReservationDTO reservationDTO)
        {
            
            var flight = await context.flights
                .Include(f => f.DepartureLocation)
                .Include(f => f.ArrivalLocation)
                .FirstOrDefaultAsync(f => f.Id == reservationDTO.FlightId);
            if (flight == null) 
            {
                Console.WriteLine("Flight not found.");
            }

            var user = await context.users.FirstOrDefaultAsync(u => u.Id == reservationDTO.UserId);
            if (user == null)
            {
                Console.WriteLine("User not found.");
            }
            
            
            flight.ReserveSeats(reservationDTO.SeatNumbers);

            var reservation = ReservationMapper.ToEntity(reservationDTO, flight, user);

            context.reservations.Add(reservation);
            await context.SaveChangesAsync();
            

            return ReservationMapper.ToResponse(reservation);
        }

        public async Task<bool> CancelReservation(long reservationId)
        {
            var reservation = await context.reservations
                .Include(r => r.Flight)
                .ThenInclude(f => f.OccupiedSeats) // Ensure OccupiedSeats are loaded!
                .FirstOrDefaultAsync(r => r.Id == reservationId);
    
            if (reservation == null) return false;
            
            var occupiedSeatsToRemove = reservation.Flight.OccupiedSeats
                .Where(s => reservation.SeatNumbers.Contains(s.Seat))
                .ToList();


            context.occupiedSeats.RemoveRange(occupiedSeatsToRemove);
            
            reservation.Flight.ReleaseSeats(reservation.SeatNumbers);
            
            context.reservations.Remove(reservation);
    
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> CalculateTotalProfit()
        {
            return await context.reservations
                .Include(r => r.Flight)
                .SumAsync(r => r.Flight.Price * r.SeatNumbers.Count);
        }

        public async Task<decimal> GetAverageTicketPrice()
        {
            var totalReservations = await context.reservations.CountAsync();
            if (totalReservations == 0) return 0m;

            var totalRevenue = await context.reservations
                .Include(r => r.Flight)
                .SumAsync(r => r.Flight.Price);
            
            return totalRevenue / totalReservations;
        }
}