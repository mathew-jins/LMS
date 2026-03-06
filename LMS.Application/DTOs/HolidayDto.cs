namespace LMS.Application.DTOs
{
    public class HolidayDto
    {
        public int HolidayId { get; set; }
        public string HolidayName { get; set; } = string.Empty;
        public DateTime HolidayDate { get; set; }
    }
}
