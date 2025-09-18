using System.ComponentModel.DataAnnotations;

namespace PharmacyService.Models.Entities
{
    public class Prescription
    {
        public int Id { get; set; }
        
        [Required]
        public int PatientId { get; set; }
        
        [Required]
        public int DoctorId { get; set; }
        
        [Required]
        public DateTime PrescriptionDate { get; set; }
        
        [MaxLength(1000)]
        public string? Notes { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, Filled, Cancelled
        
        public DateTime? FilledDate { get; set; }
        
        [MaxLength(100)]
        public string? FilledBy { get; set; } // Pharmacist name
        
        [Required]
        public DateTime CreatedAt { get; set; }
        
        [Required]
        public DateTime UpdatedAt { get; set; }
        
        [Required]
        public bool IsDeleted { get; set; } = false;
        
        // Navigation property
        public List<PatientMedicine> PatientMedicines { get; set; } = new List<PatientMedicine>();
    }
}
