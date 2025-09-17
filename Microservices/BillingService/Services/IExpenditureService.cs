using BillingService.Models.DTOs;

namespace BillingService.Services
{
    public interface IExpenditureService
    {
        Task<IEnumerable<ExpenditureDto>> GetAllExpendituresAsync();
        Task<ExpenditureDto?> GetExpenditureByIdAsync(int id);
        Task<ExpenditureDto> CreateExpenditureAsync(CreateExpenditureDto createDto);
        Task<ExpenditureDto?> UpdateExpenditureAsync(int id, UpdateExpenditureDto updateDto);
        Task<bool> DeleteExpenditureAsync(int id);
        Task<IEnumerable<ExpenditureDto>> SearchExpendituresAsync(ExpenditureSearchDto searchDto);
    }
}
