using PharmacyService.Models.DTOs;
using PharmacyService.Models.Entities;

namespace PharmacyService.Services
{
    public interface IPrescriptionService
    {
        Task<IEnumerable<PrescriptionDto>> GetAllPrescriptionsAsync();
        Task<PrescriptionDto?> GetPrescriptionByIdAsync(int id);
        Task<PrescriptionDto> CreatePrescriptionAsync(CreatePrescriptionDto createDto);
        Task<PrescriptionDto?> UpdatePrescriptionStatusAsync(int id, UpdatePrescriptionStatusDto updateDto);
        Task<bool> DeletePrescriptionAsync(int id);
        Task<IEnumerable<PrescriptionDto>> GetPrescriptionsByPatientIdAsync(int patientId);
        Task<IEnumerable<PrescriptionDto>> GetPrescriptionsByDoctorIdAsync(int doctorId);
        Task<IEnumerable<PrescriptionDto>> GetPrescriptionsByStatusAsync(string status);
        Task<IEnumerable<PrescriptionDto>> GetPrescriptionsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<PrescriptionDto?> SavePatientMedicineAsync(int prescriptionId, CreatePatientMedicineDto createDto);
    }
}


