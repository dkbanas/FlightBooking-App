namespace Application.DTOs;
// Data Transfer Object (DTO) for changing a user's role
public class ChangeRoleDTO
{
    public string Email { get; set; } = string.Empty;
    public string NewRole { get; set; } = string.Empty; // The new role to be assigned to the user
}