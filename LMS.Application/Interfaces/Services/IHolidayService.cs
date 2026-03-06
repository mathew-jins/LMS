using LMS.Application.DTOs;

namespace LMS.Application.Interfaces.Services
{
    public interface IHolidayService
    {
        Task<IEnumerable<HolidayDto>> GetAllAsync();
        Task<HolidayDto?> GetByIdAsync(int id);
        Task<HolidayDto> CreateAsync(HolidayDto dto);
        Task UpdateAsync(int id, HolidayDto dto);
        Task DeleteAsync(int id);
    }
}
