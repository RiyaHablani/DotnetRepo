namespace HospitalManagementSystem.Models.Entities
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Specialization { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool IsActive { get; set; } = true;
    }
}
