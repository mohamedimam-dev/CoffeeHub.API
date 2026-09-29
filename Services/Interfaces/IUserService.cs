using CoffeeHub.API.DTOs.Users;

namespace CoffeeHub.API.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> AddUserAsync(AddUserDto dto);

        Task<UserDto?> GetUserByIdAsync(int id);

        Task<List<UserDto>> GetAllUsersAsync();

        Task<UserDto?> UpdateUserAsync(
            int id,
            UpdateUserDto dto);

        Task<bool> ChangeCredentialsAsync(
            int id,
            ChangeCredentialsDto dto);

        Task<bool> DeleteUserAsync(int id);
    }
}
