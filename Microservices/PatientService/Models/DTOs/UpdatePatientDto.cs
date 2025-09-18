using System.ComponentModel.DataAnnotations;

namespace PatientService.Models.DTOs
{
    public class UpdatePatientDto
    {
        [MaxLength(100)]
        public string? Name { get; set; }
        
        [EmailAddress]
        [MaxLength(100)]
        public string? Email { get; set; }
        
        [Range(1, 120, ErrorMessage = "Age must be between 1 and 120")]
        public int? Age { get; set; }
        
        [MaxLength(10)]
        public string? Gender { get; set; }
        
        [MaxLength(200)]
        public string? Address { get; set; }
        
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }
        
        [MaxLength(20)]
        public string? EmergencyContact { get; set; }
        
        [MaxLength(200)]
        public string? MedicalHistory { get; set; }
        
        [MaxLength(200)]
        public string? Allergies { get; set; }
    }
}


