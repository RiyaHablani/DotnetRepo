using System.ComponentModel.DataAnnotations;

namespace PharmacyService.Models.Entities
{
    public class PatientMedicine
    {
        public int Id { get; set; }
        
        [Required]
        public int PrescriptionId { get; set; }
        
        [Required]
        public int MedicineId { get; set; }
        
        [Required]
        public int Quantity { get; set; }
        
        [MaxLength(200)]
        public string? Instructions { get; set; } // Take with food, twice daily, etc.
        
        [MaxLength(100)]
        public string? Dosage { get; set; } // 1 tablet, 2ml, etc.
        
        [MaxLength(50)]
        public string? Frequency { get; set; } // Daily, Twice daily, etc.
        
        [MaxLength(50)]
        public string? Duration { get; set; } // 7 days, 2 weeks, etc.
        
        [Required]
        public DateTime CreatedAt { get; set; }
        
        [Required]
        public DateTime UpdatedAt { get; set; }
        
        [Required]
        public bool IsDeleted { get; set; } = false;
        
        // Navigation properties
        public Prescription Prescription { get; set; } = null!;
        public Medicine Medicine { get; set; } = null!;
    }
}
