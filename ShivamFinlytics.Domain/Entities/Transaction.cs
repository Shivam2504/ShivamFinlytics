using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ShivamFinlytics.Domain.Enums;

namespace ShivamFinlytics.Domain.Entities;

public class Transaction
{
    [Key]
    public int TransactionId {get;set;}

    public int UserId {get;set;}

    public decimal Amount {get;set;}

    public TransactionType Type {get;set;}

    public int CategoryId {get;set;}

    public DateTime Date {get;set;}

    public required string Note {get;set;}

    public DateTime CreatedAt {get;set;} = DateTime.UtcNow;

    public bool IsDeleted {get;set;} = false;

    public User User {get;set;}

    public Category Category {get;set;}

}
