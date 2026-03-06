using LMS.Application.DTOs;

namespace LMS.Application.Interfaces.Services
{
    public interface ILeavePeriodService
    {
        Task<IEnumerable<LeavePeriodDto>> GetAllAsync();
        Task<LeavePeriodDto?> GetByIdAsync(int id);
        Task<LeavePeriodDto> CreateAsync(LeavePeriodDto dto);
        Task UpdateAsync(int id, LeavePeriodDto dto);
        Task DeleteAsync(int id);
        Task<LeavePeriodDto?> GetActivePeriodAsync();
    }
}
