using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces;

/// <summary>
/// Interface for reservation-related operations.
/// </summary>
public interface IReservationService
{
    /// <summary>
    /// Retrieves a list of reservations for a specified user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A list of <see cref="ReservationResponseDTO"/> for the user.</returns>
    Task<List<ReservationResponseDTO>> GetUserReservations(int userId);
    
    /// <summary>
    /// Creates a new reservation.
    /// </summary>
    /// <param name="reservationDTO">The details of the reservation to create.</param>
    /// <returns>The created <see cref="ReservationResponseDTO"/>.</returns>
    Task<ReservationResponseDTO> CreateReservation(ReservationDTO reservationDTO);
    
    /// <summary>
    /// Cancels an existing reservation.
    /// </summary>
    /// <param name="reservationId">The ID of the reservation to cancel.</param>
    /// <returns>True if the reservation was canceled successfully; otherwise, false.</returns>
    Task<bool> CancelReservation(long reservationId);
    
    /// <summary>
    /// Calculates the total profit from reservations.
    /// </summary>
    /// <returns>The total profit as a decimal.</returns>
    Task<decimal> CalculateTotalProfit();
    
    /// <summary>
    /// Retrieves the average ticket price from reservations.
    /// </summary>
    /// <returns>The average ticket price as a decimal.</returns>
    Task<decimal> GetAverageTicketPrice();
}