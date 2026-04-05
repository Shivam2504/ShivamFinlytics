using System;
using System.ComponentModel.DataAnnotations;

namespace ShivamFinlytics.Application.DTOs;

public class RegisterUserDto
{
    public required string Name {get;set;}

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public required string Email {get;set;}= string.Empty;

    public required string Password {get;set;}

    public int RoleId {get;set;}
}
