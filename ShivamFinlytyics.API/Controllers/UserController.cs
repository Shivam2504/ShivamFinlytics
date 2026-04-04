using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShivamFinlytics.Application.DTOs;
using ShivamFinlytics.Application.Interfaces;

namespace ShivamFinlytyics.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IActivityLogService _logservice;

        // Injecting the Interface, not the concrete class
        public UserController(IUserService userService, IActivityLogService logService)
        {
            _userService = userService;
            _logservice = logService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUser();
            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var user = await _userService.GetUserById(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // [Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            try
            {
                await _userService.Register(dto);
                return Ok(new { message = "User created successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("toggle-status/{id}")]
        public async Task<IActionResult> ToggleStatus(int id, [FromBody] bool isActive)
        {
            await _userService.ToggleUserStatus(id, isActive);
            return Ok(new { message = $"User status updated to {(isActive ? "Active" : "Inactive")}" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update-role/{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] int roleId)
        {
            await _userService.UpdateUserRole(id, roleId);
            return Ok(new { message = "User role updated successfully" });
        }

        [HttpGet("admin/logs")]
        [Authorize(Roles = "Admin")] // 🔒 Critical: Only users with "Admin" claim in JWT can enter
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
                // Log the error internally and return a clean message
                return StatusCode(500, new { message = "An error occurred while retrieving logs.", error = ex.Message });
            }
        }

        /// <summary>
        /// Fetches activity logs for a specific user.
        /// </summary>
        [HttpGet("admin/logs/user/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetLogsByUser(int userId)
        {
            var logs = await _logservice.GetLogsByUserIdAsync(userId);
            return Ok(logs);
        }
    }
}
