using LMS.Application.DTOs;

namespace LMS.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<UserDto?> CreateUserAsync(RegisterRequest request);
        Task UpdateUserAsync(int id, UserDto request);
        Task DeleteUserAsync(int id);
    }
}
