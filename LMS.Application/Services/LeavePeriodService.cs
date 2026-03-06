using LMS.Application.DTOs;
using LMS.Application.Interfaces;
using LMS.Application.Interfaces.Services;
using LMS.Domain.Entities;

namespace LMS.Application.Services
{
    public class LeavePeriodService : ILeavePeriodService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LeavePeriodService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<LeavePeriodDto> CreateAsync(LeavePeriodDto dto)
        {
            var entity = new LeavePeriod
            {
                StartDate = DateTime.SpecifyKind(dto.StartDate, DateTimeKind.Utc),
                EndDate = DateTime.SpecifyKind(dto.EndDate, DateTimeKind.Utc),
                Year = dto.Year,
                IsActive = dto.IsActive
            };
            await _unitOfWork.LeavePeriods.AddAsync(entity);
            await _unitOfWork.CompleteAsync();
            
            dto.LeavePeriodId = entity.LeavePeriodId;
            return dto;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.LeavePeriods.GetByIdAsync(id);
            if (entity != null)
            {
                _unitOfWork.LeavePeriods.Remove(entity);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task<LeavePeriodDto?> GetActivePeriodAsync()
        {
            var entities = await _unitOfWork.LeavePeriods.FindAsync(lp => lp.IsActive);
            var entity = entities.FirstOrDefault();
            if (entity == null) return null;
            return MapToDto(entity);
        }

        public async Task<IEnumerable<LeavePeriodDto>> GetAllAsync()
        {
            var entities = await _unitOfWork.LeavePeriods.GetAllAsync();
            return entities.Select(MapToDto);
        }

        public async Task<LeavePeriodDto?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.LeavePeriods.GetByIdAsync(id);
            if (entity == null) return null;
            return MapToDto(entity);
        }

        public async Task UpdateAsync(int id, LeavePeriodDto dto)
        {
            var entity = await _unitOfWork.LeavePeriods.GetByIdAsync(id);
            if (entity != null)
            {
                entity.StartDate = DateTime.SpecifyKind(dto.StartDate, DateTimeKind.Utc);
                entity.EndDate = DateTime.SpecifyKind(dto.EndDate, DateTimeKind.Utc);
                entity.Year = dto.Year;
                entity.IsActive = dto.IsActive;
                _unitOfWork.LeavePeriods.Update(entity);
                await _unitOfWork.CompleteAsync();
            }
        }

        private LeavePeriodDto MapToDto(LeavePeriod lp)
        {
            return new LeavePeriodDto
            {
                LeavePeriodId = lp.LeavePeriodId,
                StartDate = lp.StartDate,
                EndDate = lp.EndDate,
                Year = lp.Year,
                IsActive = lp.IsActive
            };
        }
    }
}
