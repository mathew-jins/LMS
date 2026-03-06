using LMS.Domain.Enums;

namespace LMS.Application.DTOs
{
    public class LeaveRequestDto
    {
        public int LeaveRequestId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int LeaveTypeId { get; set; }
        public string LeaveTypeName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalDays { get; set; }
        public LeaveStatus Status { get; set; }
        public string? AdminComments { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class CreateLeaveRequestDto
    {
        public int LeaveTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    
    public class ReviewLeaveRequestDto
    {
        public LeaveStatus Status { get; set; }
        public string? AdminComments { get; set; }
    }
}
