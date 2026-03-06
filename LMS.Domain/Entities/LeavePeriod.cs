using System.ComponentModel.DataAnnotations;

namespace LMS.Domain.Entities
{
    public class LeavePeriod
    {
        [Key]
        public int LeavePeriodId { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        public int Year { get; set; }
        
        public bool IsActive { get; set; } = true;
    }
}
