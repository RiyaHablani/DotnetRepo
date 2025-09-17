using PatientService.Models.DTOs;

namespace PatientService.Services
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientDto>> GetAllPatientsAsync();
        Task<PatientDto?> GetPatientByIdAsync(int id);
        Task<PatientDto> CreatePatientAsync(CreatePatientDto createDto);
        Task<PatientDto?> UpdatePatientAsync(int id, UpdatePatientDto updateDto);
        Task<bool> DeletePatientAsync(int id);
    }
}
