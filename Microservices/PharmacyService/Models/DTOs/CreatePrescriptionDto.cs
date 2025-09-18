using System.ComponentModel.DataAnnotations;

namespace PharmacyService.Models.DTOs
{
    public class CreatePrescriptionDto
    {
        [Required]
        public int PatientId { get; set; }
        
        [Required]
        public int DoctorId { get; set; }
        
        [Required]
        public DateTime PrescriptionDate { get; set; }
        
        [MaxLength(1000)]
        public string? Notes { get; set; }
        
        public List<CreatePatientMedicineDto> PatientMedicines { get; set; } = new List<CreatePatientMedicineDto>();
    }
}

