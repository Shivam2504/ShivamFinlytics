using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShivamFinlytics.Application.DTOs;
using ShivamFinlytics.Application.Interfaces;

namespace ShivamFinlytyics.API.Controllers;

/// <summary>
/// Provides administrative endpoints for user management, role assignments, 
/// and system-wide activity audit logs.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IActivityLogService _logservice;

    public UserController(IUserService userService, IActivityLogService logService)
    {
        _userService = userService;
        _logservice = logService;
    }

    /// <summary>
    /// Retrieves a list of all registered users in the system.
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllUser();
        return Ok(users);
    }

    /// <summary>
    /// Retrieves specific user details by their unique identifier.
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var user = await _userService.GetUserById(id);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Registers a new user into the system.
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        try
        {
            await _userService.Register(dto);
            return Ok(new { message = "User created successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Enables or disables a user account.
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpPut("toggle-status/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ToggleStatus(int id, [FromBody] bool isActive)
    {
        await _userService.ToggleUserStatus(id, isActive);
        return Ok(new { message = $"User status updated to {(isActive ? "Active" : "Inactive")}" });
    }

    /// <summary>
    /// Updates the assigned role for a specific user.
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpPut("update-role/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateRole(int id, [FromBody] int roleId)
    {
        await _userService.UpdateUserRole(id, roleId);
        return Ok(new { message = "User role updated successfully" });
    }

    /// <summary>
    /// Retrieves all system-wide activity logs for auditing purposes.
    /// </summary>
    [HttpGet("admin/logs")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllLogs()
    {
        try
        {
            var logs = await _logservice.GetAllLogsAsync();

            if (logs == null || !logs.Any())
            {
                return NotFound(new { message = "No activity logs found." });
            }

            return Ok(logs);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving logs.", error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves activity logs filtered by a specific user.
    /// </summary>
    [HttpGet("admin/logs/user/{userId}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLogsByUser(int userId)
    {
        var logs = await _logservice.GetLogsByUserIdAsync(userId);
        return Ok(logs);
    }
}