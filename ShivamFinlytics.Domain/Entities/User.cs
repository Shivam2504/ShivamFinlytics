using System;
using System.ComponentModel.DataAnnotations;
using ShivamFinlytics.Domain.Enums;

namespace ShivamFinlytics.Domain.Entities;

public class User
{
    [Key]
    public int UserId {get;set;}

    public required string Name{get;set;}

    public required string Email{get;set;}

    public required string PasswordHash{get;set;}

    public int RoleId{get;set;}

    public bool IsActive {get;set;} = true;

    public DateTime CreatedAt {get;set;} = DateTime.Now;

    public Role Role {get;set;} = null!;

    public ICollection<Transaction> Transactions{get;set;}

    public ICollection<ActivityLog> ActivityLogs{get;set;}

}
