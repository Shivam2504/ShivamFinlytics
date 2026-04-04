using System;
using ShivamFinlytics.Application.DTOs;
using ShivamFinlytics.Domain.Entities;

namespace ShivamFinlytics.Application.Interfaces;

public interface IUserService
{
    Task Register(RegisterUserDto dto);
    Task<List<User>> GetAllUser();

    Task<User?> GetUserById(int id);

    Task UpdateUserRole(int userId,int RoleId);

    Task ToggleUserStatus(int userId,bool IsActive);

}
