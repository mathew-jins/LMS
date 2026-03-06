using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Domain.Entities
{
    public class LeaveBalance
    {
        [Key]
        public int LeaveBalanceId { get; set; }
        
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
        
        public int LeaveTypeId { get; set; }
        [ForeignKey("LeaveTypeId")]
        public LeaveType? LeaveType { get; set; }
        
        public int TotalDays { get; set; }
        
        public int UsedDays { get; set; }
        
        public int RemainingDays { get; set; }
    }
}
