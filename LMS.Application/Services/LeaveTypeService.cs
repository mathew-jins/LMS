using LMS.Application.DTOs;
using LMS.Application.Interfaces;
using LMS.Application.Interfaces.Services;
using LMS.Domain.Entities;

namespace LMS.Application.Services
{
    public class LeaveTypeService : ILeaveTypeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LeaveTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<LeaveTypeDto> CreateAsync(LeaveTypeDto dto)
        {
            var entity = new LeaveType
            {
                LeaveTypeName = dto.LeaveTypeName,
                Description = dto.Description,
                MaxDaysPerYear = dto.MaxDaysPerYear
            };
            await _unitOfWork.LeaveTypes.AddAsync(entity);
            await _unitOfWork.CompleteAsync();
            
            dto.LeaveTypeId = entity.LeaveTypeId;
            return dto;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.LeaveTypes.GetByIdAsync(id);
            if (entity != null)
            {
                _unitOfWork.LeaveTypes.Remove(entity);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task<IEnumerable<LeaveTypeDto>> GetAllAsync()
        {
            var entities = await _unitOfWork.LeaveTypes.GetAllAsync();
            return entities.Select(e => new LeaveTypeDto
            {
                LeaveTypeId = e.LeaveTypeId,
                LeaveTypeName = e.LeaveTypeName,
                Description = e.Description,
                MaxDaysPerYear = e.MaxDaysPerYear
            });
        }

        public async Task<LeaveTypeDto?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.LeaveTypes.GetByIdAsync(id);
            if (entity == null) return null;
            return new LeaveTypeDto
            {
                LeaveTypeId = entity.LeaveTypeId,
                LeaveTypeName = entity.LeaveTypeName,
                Description = entity.Description,
                MaxDaysPerYear = entity.MaxDaysPerYear
            };
        }

        public async Task UpdateAsync(int id, LeaveTypeDto dto)
        {
            var entity = await _unitOfWork.LeaveTypes.GetByIdAsync(id);
            if (entity != null)
            {
                entity.LeaveTypeName = dto.LeaveTypeName;
                entity.Description = dto.Description;
                entity.MaxDaysPerYear = dto.MaxDaysPerYear;
                _unitOfWork.LeaveTypes.Update(entity);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
