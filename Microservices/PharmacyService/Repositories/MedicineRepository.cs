using Microsoft.EntityFrameworkCore;
using PharmacyService.Data;
using PharmacyService.Models.Entities;
using PharmacyService.Models.DTOs;

namespace PharmacyService.Repositories
{
    public class MedicineRepository : Repository<Medicine>, IMedicineRepository
    {
        public MedicineRepository(PharmacyDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Medicine>> GetAllAsync()
        {
            return await _dbSet
                .Where(m => !m.IsDeleted)
                .OrderBy(m => m.Name)
                .ToListAsync();
        }

        public override async Task<Medicine?> GetByIdAsync(int id)
        {
            return await _dbSet
                .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);
        }

        public async Task<IEnumerable<Medicine>> GetUnexpiredMedicinesAsync()
        {
            return await _dbSet
                .Where(m => !m.IsDeleted && m.ExpiryDate > DateTime.UtcNow)
                .OrderBy(m => m.ExpiryDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Medicine>> SearchMedicinesAsync(MedicineSearchDto searchDto)
        {
            var query = _dbSet.Where(m => !m.IsDeleted);

            if (!string.IsNullOrEmpty(searchDto.Name))
            {
                query = query.Where(m => m.Name.Contains(searchDto.Name));
            }

            if (!string.IsNullOrEmpty(searchDto.DiseaseType))
            {
                query = query.Where(m => m.DiseaseType.Contains(searchDto.DiseaseType));
            }

            if (searchDto.ExpiryDateFrom.HasValue)
            {
                query = query.Where(m => m.ExpiryDate >= searchDto.ExpiryDateFrom.Value);
            }

            if (searchDto.ExpiryDateTo.HasValue)
            {
                query = query.Where(m => m.ExpiryDate <= searchDto.ExpiryDateTo.Value);
            }

            if (searchDto.IncludeExpired == false)
            {
                query = query.Where(m => m.ExpiryDate > DateTime.UtcNow);
            }

            return await query
                .OrderBy(m => m.Name)
                .Skip((searchDto.PageNumber - 1) * searchDto.PageSize)
                .Take(searchDto.PageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Medicine>> GetMedicinesByDiseaseTypeAsync(string diseaseType)
        {
            return await _dbSet
                .Where(m => !m.IsDeleted && m.DiseaseType.Contains(diseaseType))
                .OrderBy(m => m.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Medicine>> GetExpiringMedicinesAsync(int daysFromNow)
        {
            var expiryThreshold = DateTime.UtcNow.AddDays(daysFromNow);
            return await _dbSet
                .Where(m => !m.IsDeleted && m.ExpiryDate <= expiryThreshold && m.ExpiryDate > DateTime.UtcNow)
                .OrderBy(m => m.ExpiryDate)
                .ToListAsync();
        }

        public async Task<bool> IsMedicineNameUniqueAsync(string name, int? excludeId = null)
        {
            var query = _dbSet.Where(m => !m.IsDeleted && m.Name.ToLower() == name.ToLower());
            
            if (excludeId.HasValue)
            {
                query = query.Where(m => m.Id != excludeId.Value);
            }

            return !await query.AnyAsync();
        }

        public override async Task DeleteAsync(Medicine entity)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}


