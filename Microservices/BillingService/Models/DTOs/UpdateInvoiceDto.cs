using System.ComponentModel.DataAnnotations;

namespace BillingService.Models.DTOs
{
    public class UpdateInvoiceDto
    {
        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than 0")]
        public decimal? TotalAmount { get; set; }
        
        [MaxLength(500)]
        public string? Description { get; set; }
        
        public DateTime? DueDate { get; set; }
    }
}


