using BillingService.Models.DTOs;

namespace BillingService.Services
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionDto>> GetAllTransactionsAsync();
        Task<TransactionDto?> GetTransactionByIdAsync(int id);
        Task<TransactionDto> CreateTransactionAsync(CreateTransactionDto createDto);
        Task<TransactionDto?> UpdateTransactionAsync(int id, UpdateTransactionDto updateDto);
        Task<bool> DeleteTransactionAsync(int id);
        Task<IEnumerable<TransactionDto>> GetTransactionsByPatientIdAsync(int patientId);
        Task<IEnumerable<TransactionDto>> SearchTransactionsAsync(TransactionSearchDto searchDto);
    }
}


