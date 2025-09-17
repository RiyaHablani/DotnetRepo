namespace PatientService.Models.DTOs
{
    public class PatientDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? EmergencyContact { get; set; }
        public string? MedicalHistory { get; set; }
        public string? Allergies { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
