using PharmacyService.Models.Entities;
using PharmacyService.Models.DTOs;

namespace PharmacyService.Repositories
{
    public interface IMedicineRepository : IRepository<Medicine>
    {
        Task<IEnumerable<Medicine>> GetUnexpiredMedicinesAsync();
        Task<IEnumerable<Medicine>> SearchMedicinesAsync(MedicineSearchDto searchDto);
        Task<IEnumerable<Medicine>> GetMedicinesByDiseaseTypeAsync(string diseaseType);
        Task<IEnumerable<Medicine>> GetExpiringMedicinesAsync(int daysFromNow);
        Task<bool> IsMedicineNameUniqueAsync(string name, int? excludeId = null);
    }
}
