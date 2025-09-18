using Microsoft.EntityFrameworkCore;
using PharmacyService.Data;
using PharmacyService.Models.Entities;

namespace PharmacyService.Repositories
{
    public class PrescriptionRepository : Repository<Prescription>, IPrescriptionRepository
    {
        public PrescriptionRepository(PharmacyDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Prescription>> GetAllAsync()
        {
            return await _dbSet
                .Where(p => !p.IsDeleted)
                .Include(p => p.PatientMedicines)
                    .ThenInclude(pm => pm.Medicine)
                .OrderByDescending(p => p.PrescriptionDate)
                .ToListAsync();
        }

        public override async Task<Prescription?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Where(p => p.Id == id && !p.IsDeleted)
                .Include(p => p.PatientMedicines)
                    .ThenInclude(pm => pm.Medicine)
                .FirstOrDefaultAsync();
        }

        public async Task<Prescription?> GetPrescriptionWithMedicinesAsync(int id)
        {
            return await _dbSet
                .Where(p => p.Id == id && !p.IsDeleted)
                .Include(p => p.PatientMedicines)
                    .ThenInclude(pm => pm.Medicine)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Prescription>> GetPrescriptionsByPatientIdAsync(int patientId)
        {
            return await _dbSet
                .Where(p => !p.IsDeleted && p.PatientId == patientId)
                .Include(p => p.PatientMedicines)
                    .ThenInclude(pm => pm.Medicine)
                .OrderByDescending(p => p.PrescriptionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Prescription>> GetPrescriptionsByDoctorIdAsync(int doctorId)
        {
            return await _dbSet
                .Where(p => !p.IsDeleted && p.DoctorId == doctorId)
                .Include(p => p.PatientMedicines)
                    .ThenInclude(pm => pm.Medicine)
                .OrderByDescending(p => p.PrescriptionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Prescription>> GetPrescriptionsByStatusAsync(string status)
        {
            return await _dbSet
                .Where(p => !p.IsDeleted && p.Status == status)
                .Include(p => p.PatientMedicines)
                    .ThenInclude(pm => pm.Medicine)
                .OrderByDescending(p => p.PrescriptionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Prescription>> GetPrescriptionsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(p => !p.IsDeleted && p.PrescriptionDate >= startDate && p.PrescriptionDate <= endDate)
                .Include(p => p.PatientMedicines)
                    .ThenInclude(pm => pm.Medicine)
                .OrderByDescending(p => p.PrescriptionDate)
                .ToListAsync();
        }

        public override async Task DeleteAsync(Prescription entity)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}


