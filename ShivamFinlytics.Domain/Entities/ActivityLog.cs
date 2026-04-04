using System;
using System.ComponentModel.DataAnnotations;

namespace ShivamFinlytics.Domain.Entities;

public class ActivityLog
{
    [Key]
    public int ActivityId {get;set;}

    public int UserId {get;set;}

    public required string Action{get;set;}

    public required string EntityName{get;set;}

    public required int EntityId {get;set;}

    public DateTime TimeStamp {get;set;} = DateTime.UtcNow;

    public required string Details {get;set;}

    public User User {get;set;}

}
