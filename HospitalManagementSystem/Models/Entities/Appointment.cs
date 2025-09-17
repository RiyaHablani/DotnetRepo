using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        
        [Required]
        public int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;
        
        [Required]
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; } = null!;
        
        [Required]
        public DateTime AppointmentDate { get; set; }
        
        [Required]
        public int Duration { get; set; } = 30; // Duration in minutes
        
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public bool IsDeleted { get; set; } = false; // For soft delete
    }
    
    public enum AppointmentStatus
    {
        Scheduled,
        Completed,
        Cancelled,
        NoShow
    }
}
