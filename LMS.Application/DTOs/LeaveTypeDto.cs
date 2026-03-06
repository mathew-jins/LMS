namespace LMS.Application.DTOs
{
    public class LeaveTypeDto
    {
        public int LeaveTypeId { get; set; }
        public string LeaveTypeName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int MaxDaysPerYear { get; set; }
    }
}
