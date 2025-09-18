using AutoMapper;
using Microsoft.EntityFrameworkCore;
using BillingService.Data;
using BillingService.Models.DTOs;
using BillingService.Models.Entities;
using BillingService.Repositories;

namespace BillingService.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IRepository<Transaction> _transactionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TransactionService> _logger;
        private readonly BillingDbContext _context;

        public TransactionService(
            IRepository<Transaction> transactionRepository,
            IMapper mapper,
            ILogger<TransactionService> logger,
            BillingDbContext context)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _logger = logger;
            _context = context;
        }

        public async Task<IEnumerable<TransactionDto>> GetAllTransactionsAsync()
        {
            try
            {
                var transactions = await _context.Transactions
                    .Where(t => !t.IsDeleted)
                    .OrderByDescending(t => t.TransactionDate)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all transactions");
                throw;
            }
        }

        public async Task<TransactionDto?> GetTransactionByIdAsync(int id)
        {
            try
            {
                var transaction = await _context.Transactions
                    .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

                return transaction == null ? null : _mapper.Map<TransactionDto>(transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving transaction with ID: {TransactionId}", id);
                throw;
            }
        }

        public async Task<TransactionDto> CreateTransactionAsync(CreateTransactionDto createDto)
        {
            try
            {
                var transaction = _mapper.Map<Transaction>(createDto);
                transaction.CreatedAt = DateTime.UtcNow;
                transaction.UpdatedAt = DateTime.UtcNow;

                var createdTransaction = await _transactionRepository.AddAsync(transaction);

                _logger.LogInformation("Transaction created successfully. ID: {TransactionId}, Patient: {PatientId}, Amount: {Amount}", 
                    createdTransaction.Id, createDto.PatientId, createDto.Amount);

                return _mapper.Map<TransactionDto>(createdTransaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating transaction for Patient: {PatientId}", createDto.PatientId);
                throw;
            }
        }

        public async Task<TransactionDto?> UpdateTransactionAsync(int id, UpdateTransactionDto updateDto)
        {
            try
            {
                var existingTransaction = await _context.Transactions
                    .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

                if (existingTransaction == null)
                    return null;

                _mapper.Map(updateDto, existingTransaction);
                existingTransaction.UpdatedAt = DateTime.UtcNow;

                await _transactionRepository.UpdateAsync(existingTransaction);

                _logger.LogInformation("Transaction updated successfully. ID: {TransactionId}", id);

                return _mapper.Map<TransactionDto>(existingTransaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating transaction with ID: {TransactionId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteTransactionAsync(int id)
        {
            try
            {
                var transaction = await _transactionRepository.GetByIdAsync(id);
                if (transaction == null || transaction.IsDeleted)
                    return false;

                transaction.IsDeleted = true;
                transaction.UpdatedAt = DateTime.UtcNow;

                await _transactionRepository.UpdateAsync(transaction);

                _logger.LogInformation("Transaction deleted successfully. ID: {TransactionId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting transaction with ID: {TransactionId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<TransactionDto>> GetTransactionsByPatientIdAsync(int patientId)
        {
            try
            {
                var transactions = await _context.Transactions
                    .Where(t => t.PatientId == patientId && !t.IsDeleted)
                    .OrderByDescending(t => t.TransactionDate)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving transactions for patient: {PatientId}", patientId);
                throw;
            }
        }

        public async Task<IEnumerable<TransactionDto>> SearchTransactionsAsync(TransactionSearchDto searchDto)
        {
            try
            {
                var query = _context.Transactions
                    .Where(t => !t.IsDeleted);

                if (searchDto.PatientId.HasValue)
                    query = query.Where(t => t.PatientId == searchDto.PatientId.Value);

                if (!string.IsNullOrEmpty(searchDto.TransactionType))
                    query = query.Where(t => t.TransactionType == searchDto.TransactionType);

                if (!string.IsNullOrEmpty(searchDto.PaymentMethod))
                    query = query.Where(t => t.PaymentMethod == searchDto.PaymentMethod);

                if (searchDto.StartDate.HasValue)
                    query = query.Where(t => t.TransactionDate >= searchDto.StartDate.Value);

                if (searchDto.EndDate.HasValue)
                    query = query.Where(t => t.TransactionDate <= searchDto.EndDate.Value);

                if (searchDto.MinAmount.HasValue)
                    query = query.Where(t => t.Amount >= searchDto.MinAmount.Value);

                if (searchDto.MaxAmount.HasValue)
                    query = query.Where(t => t.Amount <= searchDto.MaxAmount.Value);

                var transactions = await query
                    .OrderByDescending(t => t.TransactionDate)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching transactions");
                throw;
            }
        }
    }
}
