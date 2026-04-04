using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using ShivamFinlytics.Application.DTOs;
using ShivamFinlytics.Application.Interfaces;

namespace ShivamFinlytyics.API.Controllers;

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

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        try
        {
            // Pass the whole 'model' object
            var token = await _authService.Login(model);

            return Ok(new { Token = token });
        }
        catch (Exception ex)
        {
            // If login fails, return 401 Unauthorized
            return Unauthorized(new { message = ex.Message });
        }
    }
}
