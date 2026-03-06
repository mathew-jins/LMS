using System.ComponentModel.DataAnnotations;

namespace LMS.Domain.Entities
{
    public class Holiday
    {
        [Key]
        public int HolidayId { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string HolidayName { get; set; } = string.Empty;
        
        public DateTime HolidayDate { get; set; }
    }
}
