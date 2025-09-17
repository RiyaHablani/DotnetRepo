using PharmacyService.Models.DTOs;
using PharmacyService.Models.Entities;

namespace PharmacyService.Services
{
    public interface IMedicineService
    {
        Task<IEnumerable<MedicineDto>> GetAllMedicinesAsync();
        Task<MedicineDto?> GetMedicineByIdAsync(int id);
        Task<MedicineDto> CreateMedicineAsync(CreateMedicineDto createDto);
        Task<MedicineDto?> UpdateMedicineAsync(int id, UpdateMedicineDto updateDto);
        Task<bool> DeleteMedicineAsync(int id);
        Task<IEnumerable<MedicineDto>> GetUnexpiredMedicinesAsync();
        Task<IEnumerable<MedicineDto>> SearchMedicinesAsync(MedicineSearchDto searchDto);
        Task<IEnumerable<MedicineDto>> GetMedicinesByDiseaseTypeAsync(string diseaseType);
        Task<IEnumerable<MedicineDto>> GetExpiringMedicinesAsync(int daysFromNow);
        Task<bool> IsMedicineNameUniqueAsync(string name, int? excludeId = null);
    }
}
