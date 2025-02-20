namespace Application.DTOs;
// DTO for returning authentication tokens after login or refresh
public class TokenResponseDTO
{
    public string token_type { get; set; } = "Bearer";
    public required string access_token { get; set; }
    public required string refresh_token { get; set; }
    
    public required long token_expiration { get; set; }
    public required long refresh_token_expiration { get; set; }
}
