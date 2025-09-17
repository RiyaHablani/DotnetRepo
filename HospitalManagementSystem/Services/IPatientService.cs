using HospitalManagementSystem.Models.DTOs;

namespace HospitalManagementSystem.Services
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientDto>> GetAllPatientsAsync();
        Task<PatientDto?> GetPatientByIdAsync(int id);
        Task<PatientDto> CreatePatientAsync(PatientDto patientDto);
        Task<PatientDto?> UpdatePatientAsync(int id, PatientDto patientDto);
        Task<bool> DeletePatientAsync(int id);
    }
}
