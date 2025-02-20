namespace Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty; // Hashed password for security
    public string Role { get; set; } = string.Empty; // Role of the user (Admin/User)
    public string RefreshToken { get; set; } = string.Empty; // Token for refreshing authentication
    public DateTime RefreshTokenExpiryTime { get; set; } // Expiry time for refresh token
}