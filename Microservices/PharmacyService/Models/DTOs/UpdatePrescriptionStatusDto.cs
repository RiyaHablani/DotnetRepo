using System.ComponentModel.DataAnnotations;

namespace PharmacyService.Models.DTOs
{
    public class UpdatePrescriptionStatusDto
    {
        [Required]
        [StringLength(50)]
        public string Status { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string? FilledBy { get; set; }
        
        public DateTime? FilledDate { get; set; }
        
        [StringLength(500)]
        public string? Notes { get; set; }
    }
}
