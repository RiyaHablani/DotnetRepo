using System.ComponentModel.DataAnnotations;
using HospitalManagementSystem.Models.Entities;

namespace HospitalManagementSystem.Models.DTOs
{
    public class UpdateAppointmentDto
    {
        public DateTime? AppointmentDate { get; set; }
        
        [Range(15, 120, ErrorMessage = "Duration must be between 15 and 120 minutes")]
        public int? Duration { get; set; }
        
        public AppointmentStatus? Status { get; set; }
    }
}
