using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models.Entities
{
    public class Patient
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        
        [Range(0, 150)]
        public int Age { get; set; }
        
        [Required]
        [MaxLength(10)]
        public string Gender { get; set; } = null!;
        
        [Required]
        [MaxLength(250)]
        public string Address { get; set; } = null!;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false; // for soft delete
        
        // Navigation properties
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public User? User { get; set; } // Navigation to User for authentication
    }
}
