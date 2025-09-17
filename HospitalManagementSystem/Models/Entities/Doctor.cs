using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models.Entities
{
    public class Doctor
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        
        [Required]
        [MaxLength(50)]
        public string Specialization { get; set; } = null!;
        
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = null!;
        
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public User? User { get; set; } // Navigation to User for authentication
    }
}
