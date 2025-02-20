namespace Application.DTOs;

// Data Transfer Object (DTO) for requesting a new access token using a refresh token
public class RefreshTokenRequestDTO
{
    public required string RefreshToken { get; set; } // The refresh token used to obtain a new access token
}