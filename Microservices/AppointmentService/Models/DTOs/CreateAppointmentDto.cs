using System.ComponentModel.DataAnnotations;

namespace AppointmentService.Models.DTOs
{
    public class CreateAppointmentDto
    {
        [Required]
        public int PatientId { get; set; }
        
        [Required]
        public int DoctorId { get; set; }
        
        [Required]
        public DateTime AppointmentDate { get; set; }
        
        [Required]
        [Range(15, 480, ErrorMessage = "Duration must be between 15 and 480 minutes")]
        public int Duration { get; set; }
    }
}


