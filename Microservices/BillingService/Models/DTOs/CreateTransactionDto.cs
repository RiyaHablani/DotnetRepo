using System.ComponentModel.DataAnnotations;

namespace BillingService.Models.DTOs
{
    public class CreateTransactionDto
    {
        [Required]
        public int PatientId { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string TransactionType { get; set; } = string.Empty;
        
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }
        
        [MaxLength(200)]
        public string? Description { get; set; }
        
        [MaxLength(50)]
        public string? PaymentMethod { get; set; }
        
        [MaxLength(100)]
        public string? ReferenceNumber { get; set; }
        
        [Required]
        public DateTime TransactionDate { get; set; }
    }
}
