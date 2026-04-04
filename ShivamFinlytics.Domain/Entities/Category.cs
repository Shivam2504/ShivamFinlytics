using System;
using System.ComponentModel.DataAnnotations;
using ShivamFinlytics.Domain.Enums;

namespace ShivamFinlytics.Domain.Entities;

public class Category
{
    [Key]
    public int CategoryId{get;set;}

    public required string Name {get;set;}

    public TransactionType Type {get;set;}

    public ICollection<Transaction> Transactions{get;set;}


}
