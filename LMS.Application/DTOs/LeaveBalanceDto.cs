namespace LMS.Application.DTOs
{
    public class LeaveBalanceDto
    {
        public int LeaveBalanceId { get; set; }
        public int UserId { get; set; }
        public int LeaveTypeId { get; set; }
        public string LeaveTypeName { get; set; } = string.Empty;
        public int TotalDays { get; set; }
        public int UsedDays { get; set; }
        public int RemainingDays { get; set; }
    }
}
