using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<User>> register(UserDTO req) 
    {
        var user = await authService.RegisterAsync(req);
        if (user is null)
            return BadRequest("Email already exists.");
        

        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenResponseDTO>> Login(UserDTO req) 
    {
        var result = await authService.LoginAsync(req);
        if (result is null)
            return BadRequest("Invalid username or password");

        return Ok(result);
    }
    
    [HttpPut("change-role")]
    public async Task<IActionResult> ChangeUserRole([FromBody] ChangeRoleDTO req)
    {
        var success = await authService.ChangeRoleAsync(req);
    
        if (!success)
            return BadRequest("Invalid user or role.");
    
        return Ok($"User {req.Email} is now a {req.NewRole}.");
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<TokenResponseDTO>> RefreshToken(RefreshTokenRequestDTO req)
    {
        var result = await authService.RefreshTokenAsync(req);
        if(result is null || result.access_token is null || result.refresh_token is null)
            return Unauthorized("Invalid refresh token");
        
        return Ok(result);
    }

    [Authorize]
    [HttpGet]
    public IActionResult AuthenticatedOnlyEndpoint()
    {
        return Ok("You are authenticated");
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet("admin-only")]
    public IActionResult AuthenticatedAdminOnlyEndpoint()
    {
        return Ok("You are authenticated");
    }
}