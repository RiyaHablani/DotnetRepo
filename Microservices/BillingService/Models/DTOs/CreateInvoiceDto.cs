using System.ComponentModel.DataAnnotations;

namespace BillingService.Models.DTOs
{
    public class CreateInvoiceDto
    {
        [Required]
        public int PatientId { get; set; }
        
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than 0")]
        public decimal TotalAmount { get; set; }
        
        [MaxLength(500)]
        public string? Description { get; set; }
        
        [Required]
        public DateTime InvoiceDate { get; set; }
        
        public DateTime? DueDate { get; set; }
    }
}
