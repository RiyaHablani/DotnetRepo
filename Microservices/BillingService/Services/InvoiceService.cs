using AutoMapper;
using Microsoft.EntityFrameworkCore;
using BillingService.Data;
using BillingService.Models.DTOs;
using BillingService.Models.Entities;
using BillingService.Repositories;

namespace BillingService.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IRepository<Invoice> _invoiceRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<InvoiceService> _logger;
        private readonly BillingDbContext _context;

        public InvoiceService(
            IRepository<Invoice> invoiceRepository,
            IMapper mapper,
            ILogger<InvoiceService> logger,
            BillingDbContext context)
        {
            _invoiceRepository = invoiceRepository;
            _mapper = mapper;
            _logger = logger;
            _context = context;
        }

        public async Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync()
        {
            try
            {
                var invoices = await _context.Invoices
                    .Where(i => !i.IsDeleted)
                    .OrderByDescending(i => i.InvoiceDate)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<InvoiceDto>>(invoices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all invoices");
                throw;
            }
        }

        public async Task<InvoiceDto?> GetInvoiceByIdAsync(int id)
        {
            try
            {
                var invoice = await _context.Invoices
                    .FirstOrDefaultAsync(i => i.Id == id && !i.IsDeleted);

                return invoice == null ? null : _mapper.Map<InvoiceDto>(invoice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving invoice with ID: {InvoiceId}", id);
                throw;
            }
        }

        public async Task<InvoiceDto> CreateInvoiceAsync(CreateInvoiceDto createDto)
        {
            try
            {
                // Validate total amount > 0
                if (createDto.TotalAmount <= 0)
                {
                    throw new InvalidOperationException("Invoice total amount must be greater than 0");
                }

                var invoice = _mapper.Map<Invoice>(createDto);
                invoice.InvoiceNumber = await GenerateInvoiceNumberAsync();
                invoice.BalanceAmount = createDto.TotalAmount; // Initially, balance equals total
                invoice.CreatedAt = DateTime.UtcNow;
                invoice.UpdatedAt = DateTime.UtcNow;

                var createdInvoice = await _invoiceRepository.AddAsync(invoice);

                _logger.LogInformation("Invoice created successfully. ID: {InvoiceId}, Number: {InvoiceNumber}, Patient: {PatientId}, Amount: {Amount}", 
                    createdInvoice.Id, invoice.InvoiceNumber, createDto.PatientId, createDto.TotalAmount);

                return _mapper.Map<InvoiceDto>(createdInvoice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating invoice for Patient: {PatientId}", createDto.PatientId);
                throw;
            }
        }

        public async Task<InvoiceDto?> UpdateInvoiceAsync(int id, UpdateInvoiceDto updateDto)
        {
            try
            {
                var existingInvoice = await _context.Invoices
                    .FirstOrDefaultAsync(i => i.Id == id && !i.IsDeleted);

                if (existingInvoice == null)
                    return null;

                // If updating total amount, validate it's > 0
                if (updateDto.TotalAmount.HasValue && updateDto.TotalAmount.Value <= 0)
                {
                    throw new InvalidOperationException("Invoice total amount must be greater than 0");
                }

                _mapper.Map(updateDto, existingInvoice);
                
                // Recalculate balance if total amount changed
                if (updateDto.TotalAmount.HasValue)
                {
                    existingInvoice.BalanceAmount = updateDto.TotalAmount.Value - existingInvoice.PaidAmount;
                }

                existingInvoice.UpdatedAt = DateTime.UtcNow;

                await _invoiceRepository.UpdateAsync(existingInvoice);

                _logger.LogInformation("Invoice updated successfully. ID: {InvoiceId}", id);

                return _mapper.Map<InvoiceDto>(existingInvoice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating invoice with ID: {InvoiceId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteInvoiceAsync(int id)
        {
            try
            {
                var invoice = await _invoiceRepository.GetByIdAsync(id);
                if (invoice == null || invoice.IsDeleted)
                    return false;

                invoice.IsDeleted = true;
                invoice.UpdatedAt = DateTime.UtcNow;

                await _invoiceRepository.UpdateAsync(invoice);

                _logger.LogInformation("Invoice deleted successfully. ID: {InvoiceId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting invoice with ID: {InvoiceId}", id);
                throw;
            }
        }

        public async Task<InvoiceDto?> MarkInvoicePaidAsync(int id, MarkInvoicePaidDto markPaidDto)
        {
            try
            {
                var invoice = await _context.Invoices
                    .FirstOrDefaultAsync(i => i.Id == id && !i.IsDeleted);

                if (invoice == null)
                    return null;

                // Validate paid amount
                if (markPaidDto.PaidAmount <= 0)
                {
                    throw new InvalidOperationException("Paid amount must be greater than 0");
                }

                if (markPaidDto.PaidAmount > invoice.BalanceAmount)
                {
                    throw new InvalidOperationException("Paid amount cannot exceed the balance amount");
                }

                // Update invoice
                invoice.PaidAmount += markPaidDto.PaidAmount;
                invoice.BalanceAmount = invoice.TotalAmount - invoice.PaidAmount;
                invoice.UpdatedAt = DateTime.UtcNow;

                // If fully paid, update status and paid date
                if (invoice.BalanceAmount <= 0)
                {
                    invoice.Status = "Paid";
                    invoice.PaidDate = DateTime.UtcNow;
                }

                await _invoiceRepository.UpdateAsync(invoice);

                // Create transaction record
                var transaction = new Transaction
                {
                    PatientId = invoice.PatientId,
                    TransactionType = "Payment",
                    Amount = markPaidDto.PaidAmount,
                    Description = $"Payment for Invoice {invoice.InvoiceNumber}",
                    PaymentMethod = markPaidDto.PaymentMethod,
                    ReferenceNumber = markPaidDto.ReferenceNumber,
                    TransactionDate = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _context.Transactions.AddAsync(transaction);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Invoice payment processed successfully. ID: {InvoiceId}, Paid Amount: {PaidAmount}", 
                    id, markPaidDto.PaidAmount);

                return _mapper.Map<InvoiceDto>(invoice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking invoice as paid. ID: {InvoiceId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<InvoiceDto>> GetInvoicesByPatientIdAsync(int patientId)
        {
            try
            {
                var invoices = await _context.Invoices
                    .Where(i => i.PatientId == patientId && !i.IsDeleted)
                    .OrderByDescending(i => i.InvoiceDate)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<InvoiceDto>>(invoices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving invoices for patient: {PatientId}", patientId);
                throw;
            }
        }

        public async Task<IEnumerable<InvoiceDto>> SearchInvoicesAsync(InvoiceSearchDto searchDto)
        {
            try
            {
                var query = _context.Invoices
                    .Where(i => !i.IsDeleted);

                if (searchDto.PatientId.HasValue)
                    query = query.Where(i => i.PatientId == searchDto.PatientId.Value);

                if (!string.IsNullOrEmpty(searchDto.Status))
                    query = query.Where(i => i.Status == searchDto.Status);

                if (searchDto.StartDate.HasValue)
                    query = query.Where(i => i.InvoiceDate >= searchDto.StartDate.Value);

                if (searchDto.EndDate.HasValue)
                    query = query.Where(i => i.InvoiceDate <= searchDto.EndDate.Value);

                if (searchDto.MinAmount.HasValue)
                    query = query.Where(i => i.TotalAmount >= searchDto.MinAmount.Value);

                if (searchDto.MaxAmount.HasValue)
                    query = query.Where(i => i.TotalAmount <= searchDto.MaxAmount.Value);

                var invoices = await query
                    .OrderByDescending(i => i.InvoiceDate)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<InvoiceDto>>(invoices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching invoices");
                throw;
            }
        }

        private async Task<string> GenerateInvoiceNumberAsync()
        {
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month.ToString("D2");
            
            var lastInvoice = await _context.Invoices
                .Where(i => i.InvoiceNumber.StartsWith($"INV{year}{month}"))
                .OrderByDescending(i => i.InvoiceNumber)
                .FirstOrDefaultAsync();

            int nextNumber = 1;
            if (lastInvoice != null)
            {
                var lastNumber = lastInvoice.InvoiceNumber.Substring(8); // Remove "INV202401" prefix
                if (int.TryParse(lastNumber, out var parsedNumber))
                {
                    nextNumber = parsedNumber + 1;
                }
            }

            return $"INV{year}{month}{nextNumber:D4}";
        }
    }
}
