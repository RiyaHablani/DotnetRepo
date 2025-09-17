using System.ComponentModel.DataAnnotations;

namespace AppointmentService.Models.DTOs
{
    public class UpdateAppointmentDto
    {
        public DateTime? AppointmentDate { get; set; }
        
        [Range(15, 480, ErrorMessage = "Duration must be between 15 and 480 minutes")]
        public int? Duration { get; set; }
        
        [MaxLength(20)]
        public string? Status { get; set; }
    }
}
