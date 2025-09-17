using Microsoft.EntityFrameworkCore;
using PharmacyService.Data;
using PharmacyService.Models.Entities;

namespace PharmacyService.Repositories
{
    public class PatientMedicineRepository : Repository<PatientMedicine>, IPatientMedicineRepository
    {
        public PatientMedicineRepository(PharmacyDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<PatientMedicine>> GetAllAsync()
        {
            return await _dbSet
                .Where(pm => !pm.IsDeleted)
                .Include(pm => pm.Medicine)
                .Include(pm => pm.Prescription)
                .OrderBy(pm => pm.Id)
                .ToListAsync();
        }

        public override async Task<PatientMedicine?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Where(pm => pm.Id == id && !pm.IsDeleted)
                .Include(pm => pm.Medicine)
                .Include(pm => pm.Prescription)
                .FirstOrDefaultAsync();
        }

        public async Task<PatientMedicine?> GetPatientMedicineWithDetailsAsync(int id)
        {
            return await _dbSet
                .Where(pm => pm.Id == id && !pm.IsDeleted)
                .Include(pm => pm.Medicine)
                .Include(pm => pm.Prescription)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PatientMedicine>> GetPatientMedicinesByPrescriptionIdAsync(int prescriptionId)
        {
            return await _dbSet
                .Where(pm => !pm.IsDeleted && pm.PrescriptionId == prescriptionId)
                .Include(pm => pm.Medicine)
                .OrderBy(pm => pm.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<PatientMedicine>> GetPatientMedicinesByMedicineIdAsync(int medicineId)
        {
            return await _dbSet
                .Where(pm => !pm.IsDeleted && pm.MedicineId == medicineId)
                .Include(pm => pm.Medicine)
                .Include(pm => pm.Prescription)
                .OrderByDescending(pm => pm.CreatedAt)
                .ToListAsync();
        }

        public override async Task DeleteAsync(PatientMedicine entity)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}
