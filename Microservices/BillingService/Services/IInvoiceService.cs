using BillingService.Models.DTOs;

namespace BillingService.Services
{
    public interface IInvoiceService
    {
        Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync();
        Task<InvoiceDto?> GetInvoiceByIdAsync(int id);
        Task<InvoiceDto> CreateInvoiceAsync(CreateInvoiceDto createDto);
        Task<InvoiceDto?> UpdateInvoiceAsync(int id, UpdateInvoiceDto updateDto);
        Task<bool> DeleteInvoiceAsync(int id);
        Task<InvoiceDto?> MarkInvoicePaidAsync(int id, MarkInvoicePaidDto markPaidDto);
        Task<IEnumerable<InvoiceDto>> GetInvoicesByPatientIdAsync(int patientId);
        Task<IEnumerable<InvoiceDto>> SearchInvoicesAsync(InvoiceSearchDto searchDto);
    }
}


