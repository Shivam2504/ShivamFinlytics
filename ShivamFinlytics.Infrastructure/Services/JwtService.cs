using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ShivamFinlytics.Application.Interfaces; // Assuming this exists
using ShivamFinlytics.Domain.Entities;

namespace ShivamFinlytics.Infrastructure.Services;

/// <summary>
/// Service responsible for generating secure JSON Web Tokens (JWT) for authenticated users.
/// Pulls configuration directly from environment variables for high security and cloud compatibility.
/// </summary>
public class JwtService : IJwtService
{
    /// <summary>
    /// Generates a JWT token containing User ID, Email, and Role claims.
    /// </summary>
    /// <param name="user">The authenticated user entity.</param>
    /// <returns>A serialized JWT string.</returns>
    /// <exception cref="InvalidOperationException">Thrown if JWT environment variables are not configured.</exception>
    public string GenerateToken(User user)
    {
        // 🛡️ Safe retrieval with null-coalescing and descriptive errors
        var secretKey = Environment.GetEnvironmentVariable("JWT_KEY") 
            ?? throw new InvalidOperationException("Environment variable 'JWT_KEY' is not set.");
            
        var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") 
            ?? "ShivamFinlytics.API";
            
        var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") 
            ?? "ShivamFinlytics.Users";

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role?.Name ?? "User")
        };

        // Standardize the key length for HMAC-SHA256 (32 bytes / 256 bits minimum recommended)
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}