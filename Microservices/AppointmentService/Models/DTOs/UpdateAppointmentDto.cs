using System.ComponentModel.DataAnnotations;
using AppointmentService.Models.Enums;

namespace AppointmentService.Models.DTOs
{
    public class UpdateAppointmentDto
    {
        public DateTime? AppointmentDate { get; set; }
        
        [Range(15, 480, ErrorMessage = "Duration must be between 15 and 480 minutes")]
        public int? Duration { get; set; }
        
        public AppointmentStatus? Status { get; set; }
    }
}


