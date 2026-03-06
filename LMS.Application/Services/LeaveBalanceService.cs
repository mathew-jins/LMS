using LMS.Application.DTOs;
using LMS.Application.Interfaces;
using LMS.Application.Interfaces.Services;
using LMS.Domain.Entities;

namespace LMS.Application.Services
{
    public class LeaveBalanceService : ILeaveBalanceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LeaveBalanceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<LeaveBalanceDto>> GetByUserIdAsync(int userId)
        {
            var balances = await _unitOfWork.LeaveBalances.FindAsync(b => b.UserId == userId);
            
            var dtos = new List<LeaveBalanceDto>();
            foreach(var b in balances)
            {
                var leaveType = await _unitOfWork.LeaveTypes.GetByIdAsync(b.LeaveTypeId);
                dtos.Add(new LeaveBalanceDto
                {
                    LeaveBalanceId = b.LeaveBalanceId,
                    UserId = b.UserId,
                    LeaveTypeId = b.LeaveTypeId,
                    LeaveTypeName = leaveType?.LeaveTypeName ?? "Unknown",
                    TotalDays = b.TotalDays,
                    UsedDays = b.UsedDays,
                    RemainingDays = b.RemainingDays
                });
            }
            return dtos;
        }

        public async Task InitializeBalancesForUserAsync(int userId, int year)
        {
            var existing = await _unitOfWork.LeaveBalances.FindAsync(b => b.UserId == userId);
            if (existing.Any()) return;
            
            var leaveTypes = await _unitOfWork.LeaveTypes.GetAllAsync();
            foreach (var leaveType in leaveTypes)
            {
                var balance = new LeaveBalance
                {
                    UserId = userId,
                    LeaveTypeId = leaveType.LeaveTypeId,
                    TotalDays = leaveType.MaxDaysPerYear,
                    UsedDays = 0,
                    RemainingDays = leaveType.MaxDaysPerYear
                };
                await _unitOfWork.LeaveBalances.AddAsync(balance);
            }
            await _unitOfWork.CompleteAsync();
        }
    }
}
