using System;
using ShivamFinlytics.Domain.Entities;
using ShivamFinlytics.Application.DTOs;

namespace ShivamFinlytics.Application.Interfaces;

public interface ITransactionsService
{
    Task CreateTrancsaction(CreateTransactionDto dto,int userId);
    Task<List<TransactionDto>> GetAll();
    Task DeleteTransaction(int id);

}
