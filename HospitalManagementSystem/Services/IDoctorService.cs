using HospitalManagementSystem.Models.DTOs;

namespace HospitalManagementSystem.Services
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync();
        Task<DoctorDto?> GetDoctorByIdAsync(int id);
        Task<DoctorDto> CreateDoctorAsync(DoctorDto doctorDto);
        Task<DoctorDto?> UpdateDoctorAsync(int id, DoctorDto doctorDto);
        Task<bool> DeleteDoctorAsync(int id);
    }
}
