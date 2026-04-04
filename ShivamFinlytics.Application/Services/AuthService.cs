using Microsoft.EntityFrameworkCore;
using ShivamFinlytics.Application.DTOs;
using ShivamFinlytics.Application.Interfaces;
using ShivamFinlytics.Infrastructure.Data;

namespace ShivamFinlytics.Application.Services;

/// <summary>
/// Service responsible for validating user credentials and managing authentication sessions.
/// </summary>
public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IJwtService _jwtService;

    public AuthService(AppDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    /// <summary>
    /// Validates user credentials against the database and generates a secure JWT token.
    /// </summary>
    /// <param name="dto">The login credentials (email and password).</param>
    /// <returns>A signed JWT token if authentication is successful.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when email or password verification fails.</exception>
    public async Task<string> Login(LoginDto dto)
    {
        // Retrieve user with their associated Role for claim generation
        var user = await _context.Users
                                .Include(u => u.Role)
                                .FirstOrDefaultAsync(u => u.Email == dto.email);

        // Security Tip: Use generic error messages to prevent email enumeration attacks
        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        // Verify the provided plain-text password against the stored BCrypt hash
        bool isValid = BCrypt.Net.BCrypt.Verify(dto.password, user.PasswordHash);

        if (!isValid)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        // Check if user account is active (optional but recommended)
        if (!user.IsActive)
        {
            throw new UnauthorizedAccessException("Account is deactivated. Please contact an administrator.");
        }

        // Generate and return the JWT access token
        return _jwtService.GenerateToken(user);
    }
}