using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ShivamFinlytics.Domain.Entities;

public class Role
{
    [Key]
    public int RoleId {get;set;}

    public required string Name{get;set;}

    [JsonIgnore]
    public ICollection<User> Users{get;set;}

}

