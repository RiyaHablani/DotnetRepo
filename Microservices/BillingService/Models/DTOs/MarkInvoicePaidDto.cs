using System.ComponentModel.DataAnnotations;

namespace BillingService.Models.DTOs
{
    public class MarkInvoicePaidDto
    {
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Paid amount must be greater than 0")]
        public decimal PaidAmount { get; set; }
        
        [MaxLength(50)]
        public string? PaymentMethod { get; set; }
        
        [MaxLength(100)]
        public string? ReferenceNumber { get; set; }
    }
}


