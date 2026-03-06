using LMS.Application.DTOs;

namespace LMS.Application.Interfaces.Services
{
    public interface ILeaveRequestService
    {
        Task<IEnumerable<LeaveRequestDto>> GetAllAsync();
        Task<IEnumerable<LeaveRequestDto>> GetByUserIdAsync(int userId);
        Task<LeaveRequestDto?> GetByIdAsync(int id);
        Task<LeaveRequestDto> ApplyLeaveAsync(int userId, CreateLeaveRequestDto dto);
        Task ReviewLeaveAsync(int id, ReviewLeaveRequestDto dto);
    }
}
