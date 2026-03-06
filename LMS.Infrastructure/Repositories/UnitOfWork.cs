using LMS.Application.Interfaces;
using LMS.Domain.Entities;
using LMS.Infrastructure.DbContext;

namespace LMS.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        
        public IRepository<User> Users { get; private set; }
        public IRepository<LeaveType> LeaveTypes { get; private set; }
        public IRepository<LeavePeriod> LeavePeriods { get; private set; }
        public IRepository<Holiday> Holidays { get; private set; }
        public IRepository<LeaveBalance> LeaveBalances { get; private set; }
        public IRepository<LeaveRequest> LeaveRequests { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Users = new Repository<User>(_context);
            LeaveTypes = new Repository<LeaveType>(_context);
            LeavePeriods = new Repository<LeavePeriod>(_context);
            Holidays = new Repository<Holiday>(_context);
            LeaveBalances = new Repository<LeaveBalance>(_context);
            LeaveRequests = new Repository<LeaveRequest>(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
