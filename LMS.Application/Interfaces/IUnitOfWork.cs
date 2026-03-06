using LMS.Domain.Entities;

namespace LMS.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<LeaveType> LeaveTypes { get; }
        IRepository<LeavePeriod> LeavePeriods { get; }
        IRepository<Holiday> Holidays { get; }
        IRepository<LeaveBalance> LeaveBalances { get; }
        IRepository<LeaveRequest> LeaveRequests { get; }
        
        Task<int> CompleteAsync();
    }
}
