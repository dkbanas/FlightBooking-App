using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

/// <summary>
/// Provides authentication services including user registration, login, role management, and token handling.
/// </summary>
public class AuthService(AppDbContext context,IConfiguration configuration) : IAuthService
{
    
    /// <summary>
    /// Registers a new user asynchronously.
    /// </summary>
    /// <param name="req">The user registration details.</param>
    /// <returns>The registered user or null if the email is already in use.</returns>
    public async Task<User?> RegisterAsync(UserDTO req)
    {
        if (await context.users.AnyAsync(u => u.Email == req.Email))
        {
            return null;
        }

        var user = new User();
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(req.Password);


        user.Email = req.Email;
        user.Role = "User";
        user.PasswordHash = hashedPassword;

        context.users.Add(user);
        await context.SaveChangesAsync();

        return user;
    }

    /// <summary>
    /// Authenticates a user and generates a token response asynchronously.
    /// </summary>
    /// <param name="req">The user login details.</param>
    /// <returns>A token response containing access and refresh tokens or null if login fails.</returns>
    public async Task<TokenResponseDTO?> LoginAsync(UserDTO req)
    {
        var user = await context.users.FirstOrDefaultAsync(u => u.Email == req.Email);
        if (user is null)
        {
            return null;
        }
        if (!BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
        {
            return null;
        }
        
        return await CreateTokenResponse(user);
    }
    
    /// <summary>
    /// Changes the role of a user asynchronously.
    /// </summary>
    /// <param name="req">The request containing user email and new role.</param>
    /// <returns>True if the role was successfully changed; otherwise, false.</returns>
    public async Task<bool> ChangeRoleAsync(ChangeRoleDTO req)
    {
        var user = await context.users.FirstOrDefaultAsync(u => u.Email == req.Email);
        if (user is null)
            return false;

        if (req.NewRole != "Admin" && req.NewRole != "User")
            return false;

        user.Role = req.NewRole;
        await context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Creates a token response containing the access token and refresh token for the specified user.
    /// </summary>
    /// <param name="user">The user for whom the token response is created.</param>
    /// <returns>A <see cref="TokenResponseDTO"/> containing the access token and refresh token.</returns>
    private async Task<TokenResponseDTO> CreateTokenResponse(User user)
    {
        var accessTokenExpiration = DateTime.UtcNow.AddDays(1);
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(7);
        
        
        var response = new TokenResponseDTO
        {
            access_token = CreateToken(user, accessTokenExpiration),
            refresh_token = await GenerateAndSaveRefreshToken(user, refreshTokenExpiration),
            token_expiration = ((DateTimeOffset)accessTokenExpiration).ToUnixTimeSeconds(),
            refresh_token_expiration = ((DateTimeOffset)refreshTokenExpiration).ToUnixTimeSeconds()
        };
        return response;
    }

    /// <summary>
    /// Refreshes the authentication tokens based on the provided refresh token request.
    /// </summary>
    /// <param name="req">The request containing the refresh token.</param>
    /// <returns>A <see cref="TokenResponseDTO"/> with new tokens if the refresh token is valid; otherwise, null.</returns>
    public async Task<TokenResponseDTO?> RefreshTokenAsync(RefreshTokenRequestDTO req)
    {
        var user = await ValidateRefreshTokenAsync(req.RefreshToken);
        if (user is null)
            return null;
        return await CreateTokenResponse(user);
    }

    /// <summary>
    /// Validates the provided refresh token and retrieves the associated user if valid.
    /// </summary>
    /// <param name="refreshToken">The refresh token to validate.</param>
    /// <returns>The user associated with the refresh token if valid; otherwise, null.</returns>
    private async Task<User?> ValidateRefreshTokenAsync(string refreshToken)
    {
        var user = await context.users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        if(user is null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
        {
            return null;
        }
        return user;
    }

    /// <summary>
    /// Creates a JWT token for the specified user with the provided expiration time.
    /// </summary>
    /// <param name="user">The user for whom to create the token.</param>
    /// <param name="expiration">The expiration time of the token.</param>
    /// <returns>The generated token as a string.</returns>
    private string CreateToken(User user,DateTime expiration)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name,user.Email),
            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            new Claim(ClaimTypes.Role,user.Role)
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Token"]!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
        var tokenDescriptor = new JwtSecurityToken(
            issuer: configuration["JWT:Issuer"],
            audience: configuration["JWT:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    /// <summary>
    /// Generates a random refresh token.
    /// </summary>
    /// <returns>The generated refresh token as a string.</returns>
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    /// <summary>
    /// Generates and saves a new refresh token for the specified user with an expiration time.
    /// </summary>
    /// <param name="user">The user to associate the refresh token with.</param>
    /// <param name="refreshTokenExpiration">The expiration time for the refresh token.</param>
    /// <returns>The generated refresh token as a string.</returns>
    private async Task<string> GenerateAndSaveRefreshToken(User user,DateTime refreshTokenExpiration)
    {
        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = refreshTokenExpiration;
        await context.SaveChangesAsync();
        return refreshToken;
    }
}