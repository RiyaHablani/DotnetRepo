using AutoMapper;
using Microsoft.EntityFrameworkCore;
using BillingService.Data;
using BillingService.Models.DTOs;
using BillingService.Models.Entities;
using BillingService.Repositories;

namespace BillingService.Services
{
    public class ExpenditureService : IExpenditureService
    {
        private readonly IRepository<Expenditure> _expenditureRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ExpenditureService> _logger;
        private readonly BillingDbContext _context;

        public ExpenditureService(
            IRepository<Expenditure> expenditureRepository,
            IMapper mapper,
            ILogger<ExpenditureService> logger,
            BillingDbContext context)
        {
            _expenditureRepository = expenditureRepository;
            _mapper = mapper;
            _logger = logger;
            _context = context;
        }

        public async Task<IEnumerable<ExpenditureDto>> GetAllExpendituresAsync()
        {
            try
            {
                var expenditures = await _context.Expenditures
                    .Where(e => !e.IsDeleted)
                    .OrderByDescending(e => e.ExpenditureDate)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<ExpenditureDto>>(expenditures);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all expenditures");
                throw;
            }
        }

        public async Task<ExpenditureDto?> GetExpenditureByIdAsync(int id)
        {
            try
            {
                var expenditure = await _context.Expenditures
                    .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

                return expenditure == null ? null : _mapper.Map<ExpenditureDto>(expenditure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving expenditure with ID: {ExpenditureId}", id);
                throw;
            }
        }

        public async Task<ExpenditureDto> CreateExpenditureAsync(CreateExpenditureDto createDto)
        {
            try
            {
                var expenditure = _mapper.Map<Expenditure>(createDto);
                expenditure.CreatedAt = DateTime.UtcNow;
                expenditure.UpdatedAt = DateTime.UtcNow;

                var createdExpenditure = await _expenditureRepository.AddAsync(expenditure);

                _logger.LogInformation("Expenditure created successfully. ID: {ExpenditureId}, Category: {Category}, Amount: {Amount}", 
                    createdExpenditure.Id, createDto.Category, createDto.Amount);

                return _mapper.Map<ExpenditureDto>(createdExpenditure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating expenditure for Category: {Category}", createDto.Category);
                throw;
            }
        }

        public async Task<ExpenditureDto?> UpdateExpenditureAsync(int id, UpdateExpenditureDto updateDto)
        {
            try
            {
                var existingExpenditure = await _context.Expenditures
                    .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

                if (existingExpenditure == null)
                    return null;

                _mapper.Map(updateDto, existingExpenditure);
                existingExpenditure.UpdatedAt = DateTime.UtcNow;

                await _expenditureRepository.UpdateAsync(existingExpenditure);

                _logger.LogInformation("Expenditure updated successfully. ID: {ExpenditureId}", id);

                return _mapper.Map<ExpenditureDto>(existingExpenditure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating expenditure with ID: {ExpenditureId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteExpenditureAsync(int id)
        {
            try
            {
                var expenditure = await _expenditureRepository.GetByIdAsync(id);
                if (expenditure == null || expenditure.IsDeleted)
                    return false;

                expenditure.IsDeleted = true;
                expenditure.UpdatedAt = DateTime.UtcNow;

                await _expenditureRepository.UpdateAsync(expenditure);

                _logger.LogInformation("Expenditure deleted successfully. ID: {ExpenditureId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting expenditure with ID: {ExpenditureId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<ExpenditureDto>> SearchExpendituresAsync(ExpenditureSearchDto searchDto)
        {
            try
            {
                var query = _context.Expenditures
                    .Where(e => !e.IsDeleted);

                if (!string.IsNullOrEmpty(searchDto.Category))
                    query = query.Where(e => e.Category == searchDto.Category);

                if (!string.IsNullOrEmpty(searchDto.Vendor))
                    query = query.Where(e => e.Vendor == searchDto.Vendor);

                if (searchDto.StartDate.HasValue)
                    query = query.Where(e => e.ExpenditureDate >= searchDto.StartDate.Value);

                if (searchDto.EndDate.HasValue)
                    query = query.Where(e => e.ExpenditureDate <= searchDto.EndDate.Value);

                if (searchDto.MinAmount.HasValue)
                    query = query.Where(e => e.Amount >= searchDto.MinAmount.Value);

                if (searchDto.MaxAmount.HasValue)
                    query = query.Where(e => e.Amount <= searchDto.MaxAmount.Value);

                var expenditures = await query
                    .OrderByDescending(e => e.ExpenditureDate)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<ExpenditureDto>>(expenditures);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching expenditures");
                throw;
            }
        }
    }
}


