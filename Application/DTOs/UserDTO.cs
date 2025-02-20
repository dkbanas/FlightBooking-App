namespace Application.DTOs;
// Data Transfer Object (DTO) for user authentication (registration & login) request
public class UserDTO
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}