using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Models.DTOs
{
    public class PatientDto
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Range(0, 150)]
        public int Age { get; set; }
        
        [Required]
        [MaxLength(10)]
        public string Gender { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(250)]
        public string Address { get; set; } = string.Empty;
    }
}
