using System;
using ShivamFinlytics.Application.DTOs;

namespace ShivamFinlytics.Application.Interfaces;

public interface IAuthService
{
  Task<string> Login(LoginDto dto);

}
