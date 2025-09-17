using Microsoft.EntityFrameworkCore;
using AuthService.Data;
using AuthService.Models.Entities;
using System.Linq.Expressions;

namespace AuthService.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly HospitalDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(HospitalDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Patient))
            {
                return (IEnumerable<T>)await _context.Patients.Where(p => !p.IsDeleted).ToListAsync();
            }
            else if (typeof(T) == typeof(Doctor))
            {
                return (IEnumerable<T>)await _context.Doctors.Where(d => d.IsActive).ToListAsync();
            }
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            if (typeof(T) == typeof(Patient))
            {
                var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
                return patient as T;
            }
            else if (typeof(T) == typeof(Doctor))
            {
                var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == id && d.IsActive);
                return doctor as T;
            }
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (typeof(T) == typeof(Patient))
            {
                var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
                if (patient != null)
                {
                    patient.IsDeleted = true; // Soft delete
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            else if (typeof(T) == typeof(Doctor))
            {
                var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == id && d.IsActive);
                if (doctor != null)
                {
                    doctor.IsActive = false; // Soft delete
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            return false;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            if (typeof(T) == typeof(Patient))
            {
                return await _context.Patients.AnyAsync(p => p.Id == id && !p.IsDeleted);
            }
            else if (typeof(T) == typeof(Doctor))
            {
                return await _context.Doctors.AnyAsync(d => d.Id == id && d.IsActive);
            }
            return await _dbSet.AnyAsync(e => EF.Property<int>(e, "Id") == id);
        }
    }
}
