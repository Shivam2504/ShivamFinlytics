using System;
using ShivamFinlytics.Domain.Enums;

namespace ShivamFinlytics.Domain.Entities;

public class AnalystInsight
{
    public int Id {get;set;}

    public required string Title {get;set;}

    public string? Description {get;set;}

    public int CreatedBy {get;set;}

    public int? CategoryId {get;set;}

    public int? TranctionId {get;set;}

    public ImpactLevel ImpactLevel {get;set;}

    public DateTime CreatedAt{get;set;} = DateTime.UtcNow;

    public User User {get;set;}

    public Category Category {get;set;}

    public Transaction Transaction {get;set;}

}
