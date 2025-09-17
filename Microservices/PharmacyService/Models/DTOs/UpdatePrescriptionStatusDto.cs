using System.ComponentModel.DataAnnotations;

namespace PharmacyService.Models.DTOs
{
    public class UpdatePrescriptionStatusDto
    {
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = string.Empty; // Pending, Filled, Cancelled
        
        [MaxLength(100)]
        public string? FilledBy { get; set; } // Pharmacist name
    }
}
