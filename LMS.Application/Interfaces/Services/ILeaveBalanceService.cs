using LMS.Application.DTOs;

namespace LMS.Application.Interfaces.Services
{
    public interface ILeaveBalanceService
    {
        Task<IEnumerable<LeaveBalanceDto>> GetByUserIdAsync(int userId);
        Task InitializeBalancesForUserAsync(int userId, int year);
    }
}
