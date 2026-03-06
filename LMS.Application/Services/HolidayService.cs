using LMS.Application.DTOs;
using LMS.Application.Interfaces;
using LMS.Application.Interfaces.Services;
using LMS.Domain.Entities;

namespace LMS.Application.Services
{
    public class HolidayService : IHolidayService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HolidayService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<HolidayDto> CreateAsync(HolidayDto dto)
        {
            var entity = new Holiday
            {
                HolidayName = dto.HolidayName,
                HolidayDate = dto.HolidayDate
            };
            await _unitOfWork.Holidays.AddAsync(entity);
            await _unitOfWork.CompleteAsync();
            
            dto.HolidayId = entity.HolidayId;
            return dto;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Holidays.GetByIdAsync(id);
            if (entity != null)
            {
                _unitOfWork.Holidays.Remove(entity);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task<IEnumerable<HolidayDto>> GetAllAsync()
        {
            var entities = await _unitOfWork.Holidays.GetAllAsync();
            return entities.Select(e => new HolidayDto
            {
                HolidayId = e.HolidayId,
                HolidayName = e.HolidayName,
                HolidayDate = e.HolidayDate
            });
        }

        public async Task<HolidayDto?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Holidays.GetByIdAsync(id);
            if (entity == null) return null;
            return new HolidayDto
            {
                HolidayId = entity.HolidayId,
                HolidayName = entity.HolidayName,
                HolidayDate = entity.HolidayDate
            };
        }

        public async Task UpdateAsync(int id, HolidayDto dto)
        {
            var entity = await _unitOfWork.Holidays.GetByIdAsync(id);
            if (entity != null)
            {
                entity.HolidayName = dto.HolidayName;
                entity.HolidayDate = dto.HolidayDate;
                _unitOfWork.Holidays.Update(entity);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
