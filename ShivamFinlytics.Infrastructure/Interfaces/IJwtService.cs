using ShivamFinlytics.Domain.Entities;

namespace ShivamFinlytics.Application.Interfaces;

/// <summary>
/// Defines the contract for JWT generation services.
/// </summary>
public interface IJwtService
{
    string GenerateToken(User user);
}