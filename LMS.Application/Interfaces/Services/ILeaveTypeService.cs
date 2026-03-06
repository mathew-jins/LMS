using LMS.Application.DTOs;

namespace LMS.Application.Interfaces.Services
{
    public interface ILeaveTypeService
    {
        Task<IEnumerable<LeaveTypeDto>> GetAllAsync();
        Task<LeaveTypeDto?> GetByIdAsync(int id);
        Task<LeaveTypeDto> CreateAsync(LeaveTypeDto dto);
        Task UpdateAsync(int id, LeaveTypeDto dto);
        Task DeleteAsync(int id);
    }
}
