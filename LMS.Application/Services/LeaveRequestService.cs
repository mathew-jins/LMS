using LMS.Application.DTOs;
using LMS.Application.Interfaces;
using LMS.Application.Interfaces.Services;
using LMS.Domain.Entities;
using LMS.Domain.Enums;

namespace LMS.Application.Services
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LeaveRequestService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<LeaveRequestDto> ApplyLeaveAsync(int userId, CreateLeaveRequestDto dto)
        {
            var holidays = await _unitOfWork.Holidays.GetAllAsync();
            var holidayDates = holidays.Select(h => h.HolidayDate.Date).ToList();

            int totalDays = 0;
            for (var date = dto.StartDate.Date; date <= dto.EndDate.Date; date = date.AddDays(1))
            {
                if (date.DayOfWeek != DayOfWeek.Saturday && 
                    date.DayOfWeek != DayOfWeek.Sunday && 
                    !holidayDates.Contains(date))
                {
                    totalDays++;
                }
            }

            var balances = await _unitOfWork.LeaveBalances.FindAsync(b => b.UserId == userId && b.LeaveTypeId == dto.LeaveTypeId);
            var balance = balances.FirstOrDefault();

            if (balance == null || balance.RemainingDays < totalDays)
            {
                throw new InvalidOperationException("Insufficient leave balance.");
            }

            var request = new LeaveRequest
            {
                UserId = userId,
                LeaveTypeId = dto.LeaveTypeId,
                StartDate = DateTime.SpecifyKind(dto.StartDate, DateTimeKind.Utc),
                EndDate = DateTime.SpecifyKind(dto.EndDate, DateTimeKind.Utc),
                TotalDays = totalDays,
                Status = LeaveStatus.Pending,
                CreatedDate = DateTime.UtcNow
            };

            await _unitOfWork.LeaveRequests.AddAsync(request);
            await _unitOfWork.CompleteAsync();

            return await GetByIdAsync(request.LeaveRequestId) ?? new LeaveRequestDto();
        }

        public async Task<IEnumerable<LeaveRequestDto>> GetAllAsync()
        {
            var requests = await _unitOfWork.LeaveRequests.GetAllAsync();
            var dtos = new List<LeaveRequestDto>();
            foreach (var r in requests)
            {
                dtos.Add(await MapToDto(r));
            }
            return dtos;
        }

        public async Task<LeaveRequestDto?> GetByIdAsync(int id)
        {
            var r = await _unitOfWork.LeaveRequests.GetByIdAsync(id);
            if (r == null) return null;
            return await MapToDto(r);
        }

        public async Task<IEnumerable<LeaveRequestDto>> GetByUserIdAsync(int userId)
        {
            var requests = await _unitOfWork.LeaveRequests.FindAsync(r => r.UserId == userId);
            var dtos = new List<LeaveRequestDto>();
            foreach (var r in requests)
            {
                dtos.Add(await MapToDto(r));
            }
            return dtos;
        }

        public async Task ReviewLeaveAsync(int id, ReviewLeaveRequestDto dto)
        {
            var request = await _unitOfWork.LeaveRequests.GetByIdAsync(id);
            if (request == null) throw new KeyNotFoundException("Leave request not found");

            if (request.Status == LeaveStatus.Pending && dto.Status == LeaveStatus.Approved)
            {
                var balances = await _unitOfWork.LeaveBalances.FindAsync(b => b.UserId == request.UserId && b.LeaveTypeId == request.LeaveTypeId);
                var balance = balances.FirstOrDefault();
                if (balance != null)
                {
                    balance.UsedDays += request.TotalDays;
                    balance.RemainingDays -= request.TotalDays;
                    _unitOfWork.LeaveBalances.Update(balance);
                }
            }

            request.Status = dto.Status;
            request.AdminComments = dto.AdminComments;

            _unitOfWork.LeaveRequests.Update(request);
            await _unitOfWork.CompleteAsync();
        }

        private async Task<LeaveRequestDto> MapToDto(LeaveRequest r)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(r.UserId);
            var leaveType = await _unitOfWork.LeaveTypes.GetByIdAsync(r.LeaveTypeId);
            
            return new LeaveRequestDto
            {
                LeaveRequestId = r.LeaveRequestId,
                UserId = r.UserId,
                UserName = $"{user?.FirstName} {user?.LastName}",
                LeaveTypeId = r.LeaveTypeId,
                LeaveTypeName = leaveType?.LeaveTypeName ?? "Unknown",
                StartDate = r.StartDate,
                EndDate = r.EndDate,
                TotalDays = r.TotalDays,
                Status = r.Status,
                AdminComments = r.AdminComments,
                CreatedDate = r.CreatedDate
            };
        }
    }
}
