using System;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.EntityFrameworkCore;
using ShivamFinlytics.Application.DTOs;
using ShivamFinlytics.Application.Interfaces;
using ShivamFinlytics.Domain.Entities;
using ShivamFinlytics.Infrastructure.Data;
using ShivamFinlytics.Infrastructure.Services;

namespace ShivamFinlytics.Application.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly JwtService _jwtService;

    public AuthService(AppDbContext context,JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }
    public async Task<string> Login(LoginDto dto)
    {
        var user = await _context.Users
                                    .Include(u => u.Role)
                                    .FirstOrDefaultAsync(u => u.Email == dto.email);

        

        if(user == null)
        {
            throw new Exception("Invlaid userid or password");
        }

        bool isValid = BCrypt.Net.BCrypt.Verify(dto.password, user.PasswordHash);

        if (!isValid)
        {
            Console.WriteLine($"Password from DTO: {dto.password}");
            throw new Exception("Invalid userId or Password");
        }

        var token = _jwtService.GenerateToken(user);
        return token; 
    }

}
