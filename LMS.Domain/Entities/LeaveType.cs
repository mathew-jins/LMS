using System.ComponentModel.DataAnnotations;

namespace LMS.Domain.Entities
{
    public class LeaveType
    {
        [Key]
        public int LeaveTypeId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string LeaveTypeName { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
        
        public int MaxDaysPerYear { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public ICollection<LeaveBalance> LeaveBalances { get; set; } = new List<LeaveBalance>();
        public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
    }
}
