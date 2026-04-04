using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ShivamFinlytics.Application.DTOs;
using ShivamFinlytics.Application.Interfaces;
using ShivamFinlytics.Domain.Entities;
using ShivamFinlytics.Infrastructure.Data;

namespace ShivamFinlytics.Application.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }


    public async Task<List<User>> GetAllUser()
    {
        return await _context.Users
                    .Include(u => u.Role)
                    .ToListAsync();
   
    }

    public Task<User?> GetUserById(int id)
    {
        if(id <= 0)
        {
            throw new Exception("User id can not be less than 0");
        }

        var user = _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == id);
        if(user == null)
        {
            throw new Exception("User no found");
        }
        return user;
    }

    public async Task Register(RegisterUserDto dto)
    {
        var exists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
        if(exists)
        {
            throw new Exception("User alerady exist");
        }
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = passwordHash,
            RoleId = dto.RoleId
        };

        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public async Task ToggleUserStatus(int userId, bool IsActive)
    {
        if(userId <= 0)
        {
            throw new Exception("Please enter correct userId");
        }
        var user = await _context.Users.FindAsync(userId);

        if(user == null)
        {
            throw new Exception("User not found");
        }

        user.IsActive = IsActive;
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserRole(int userId, int RoleId)
    {
        if(userId <= 0)
        {
            throw new Exception("User id is worng plealse enter the correct userId");
        }
        var user = await _context.Users.FindAsync(userId);
        if(user == null)
        {
            throw new Exception("User not found");
        }

        user.RoleId = RoleId;
        await _context.SaveChangesAsync();
    }
}
