using LMS.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Domain.Entities
{
    public class LeaveRequest
    {
        [Key]
        public int LeaveRequestId { get; set; }
        
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
        
        public int LeaveTypeId { get; set; }
        [ForeignKey("LeaveTypeId")]
        public LeaveType? LeaveType { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        public int TotalDays { get; set; }
        
        public LeaveStatus Status { get; set; } = LeaveStatus.Pending;
        
        [MaxLength(500)]
        public string? AdminComments { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
