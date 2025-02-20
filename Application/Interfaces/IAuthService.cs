using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces;

/// <summary>
/// Interface for authentication-related operations.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="req">The user details for registration.</param>
    /// <returns>The registered <see cref="User"/> or null if registration fails.</returns>
    Task<User?> RegisterAsync(UserDTO req);
    
    /// <summary>
    /// Logs in a user and returns a token response.
    /// </summary>
    /// <param name="req">The user login details.</param>
    /// <returns>A <see cref="TokenResponseDTO"/> containing the access and refresh tokens.</returns>
    Task<TokenResponseDTO?> LoginAsync(UserDTO req);
    
    /// <summary>
    /// Changes the role of a user identified by their email.
    /// </summary>
    /// <param name="req">The new role details</param>
    /// <returns>True if the role was changed successfully; otherwise, false.</returns>
    Task<bool> ChangeRoleAsync(ChangeRoleDTO req);
    
    /// <summary>
    /// Refreshes the user's access token using a refresh token.
    /// </summary>
    /// <param name="req">The refresh token request details.</param>
    /// <returns>A <see cref="TokenResponseDTO"/> containing new tokens or null if refresh fails.</returns>
    Task<TokenResponseDTO?> RefreshTokenAsync(RefreshTokenRequestDTO req);
}