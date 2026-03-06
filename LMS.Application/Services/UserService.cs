using LMS.Application.DTOs;
using LMS.Application.Interfaces;
using LMS.Application.Interfaces.Services;
using LMS.Domain.Entities;

namespace LMS.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILeaveBalanceService _leaveBalanceService;

        public UserService(IUnitOfWork unitOfWork, ILeaveBalanceService leaveBalanceService)
        {
            _unitOfWork = unitOfWork;
            _leaveBalanceService = leaveBalanceService;
        }

        public async Task<UserDto?> CreateUserAsync(RegisterRequest request)
        {
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Role = request.Role,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };
            
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            // Automatically initialize balances for the newly created user
            var currentYear = DateTime.UtcNow.Year;
            await _leaveBalanceService.InitializeBalancesForUserAsync(user.UserId, currentYear);

            return MapToDto(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user != null)
            {
                user.IsActive = false; // Soft delete
                _unitOfWork.Users.Update(user);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            return users.Select(MapToDto);
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            return user != null ? MapToDto(user) : null;
        }

        public async Task UpdateUserAsync(int id, UserDto request)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user != null)
            {
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.Role = request.Role;
                user.IsActive = request.IsActive;
                _unitOfWork.Users.Update(user);
                await _unitOfWork.CompleteAsync();
            }
        }

        private UserDto MapToDto(User user)
        {
            return new UserDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedDate = user.CreatedDate
            };
        }
    }
}
