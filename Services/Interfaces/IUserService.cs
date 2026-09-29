using CoffeeHub.API.Common;
using CoffeeHub.API.DTOs.Users;

namespace CoffeeHub.API.Services.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResult<UserDto>> AddUserAsync(AddUserDto dto);

        Task<ServiceResult<UserDto>> GetUserByIdAsync(int id);

        Task<List<UserDto>> GetAllUsersAsync();

        Task<ServiceResult<UserDto>> UpdateUserAsync(
            int id,
            UpdateUserDto dto);

        Task<ServiceResult<bool>> ChangeCredentialsAsync(
            int id,
            ChangeCredentialsDto dto);

        Task<ServiceResult<bool>> DeleteUserAsync(int id);
    }
}
