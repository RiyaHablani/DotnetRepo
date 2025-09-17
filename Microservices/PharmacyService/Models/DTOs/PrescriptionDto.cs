namespace PharmacyService.Models.DTOs
{
    public class PrescriptionDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime PrescriptionDate { get; set; }
        public string? Notes { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? FilledDate { get; set; }
        public string? FilledBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<PatientMedicineDto> PatientMedicines { get; set; } = new List<PatientMedicineDto>();
    }
}
