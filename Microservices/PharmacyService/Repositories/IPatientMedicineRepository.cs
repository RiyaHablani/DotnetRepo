using PharmacyService.Models.Entities;

namespace PharmacyService.Repositories
{
    public interface IPatientMedicineRepository : IRepository<PatientMedicine>
    {
        Task<IEnumerable<PatientMedicine>> GetPatientMedicinesByPrescriptionIdAsync(int prescriptionId);
        Task<IEnumerable<PatientMedicine>> GetPatientMedicinesByMedicineIdAsync(int medicineId);
        Task<PatientMedicine?> GetPatientMedicineWithDetailsAsync(int id);
    }
}


