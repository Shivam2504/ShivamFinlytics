using System;

namespace ShivamFinlytics.Application.DTOs;

public class RegisterUserDto
{
    public required string Name {get;set;}
    public required string Email {get;set;}

    public required string Password {get;set;}

    public int RoleId {get;set;}
}
