using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models.DTOs
{
    public class CreateAppointmentDto
    {
        [Required]
        public int PatientId { get; set; }
        
        [Required]
        public int DoctorId { get; set; }
        
        [Required]
        public DateTime AppointmentDate { get; set; }
        
        [Range(15, 120, ErrorMessage = "Duration must be between 15 and 120 minutes")]
        public int Duration { get; set; } = 30;
    }
}
