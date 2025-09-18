using System.ComponentModel.DataAnnotations;

namespace DoctorService.Models.DTOs
{
    public class UpdateDoctorDto
    {
        [MaxLength(100)]
        public string? Name { get; set; }
        
        [MaxLength(100)]
        public string? Specialization { get; set; }
        
        [EmailAddress]
        [MaxLength(100)]
        public string? Email { get; set; }
        
        public bool? IsActive { get; set; }
    }
}
