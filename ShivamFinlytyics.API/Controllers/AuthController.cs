using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using ShivamFinlytics.Application.DTOs;
using ShivamFinlytics.Application.Interfaces;

namespace ShivamFinlytyics.API.Controllers;

/// <summary>
/// Provides endpoints for user authentication and authorization.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting("FixedPolicy")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Authenticates a user and returns a JWT access token.
    /// </summary>
    /// <param name="model">The user's login credentials.</param>
    /// <returns>A JWT token if successful; otherwise, an unauthorized error.</returns>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        try
        {
            var token = await _authService.Login(model);
            return Ok(new { Token = token });
        }
        catch (Exception ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }
}