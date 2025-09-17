using System.ComponentModel.DataAnnotations;

namespace BillingService.Models.DTOs
{
    public class UpdateTransactionDto
    {
        [MaxLength(50)]
        public string? TransactionType { get; set; }
        
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal? Amount { get; set; }
        
        [MaxLength(200)]
        public string? Description { get; set; }
        
        [MaxLength(50)]
        public string? PaymentMethod { get; set; }
        
        [MaxLength(100)]
        public string? ReferenceNumber { get; set; }
        
        public DateTime? TransactionDate { get; set; }
    }
}
