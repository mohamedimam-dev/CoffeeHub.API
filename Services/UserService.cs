using CoffeeHub.API.Common;
using CoffeeHub.API.Data;
using CoffeeHub.API.DTOs.Users;
using CoffeeHub.API.Entities;
using CoffeeHub.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoffeeHub.API.Services
{
    public class UserService : IUserService
    {
        private readonly CoffeeHubDbContext _context;

        public UserService(CoffeeHubDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<UserDto>> AddUserAsync(AddUserDto dto)
        {
            bool userExists = await _context.Users
                .AnyAsync(u => u.Username == dto.Username);

            if (userExists)
            {
                return ServiceResult<UserDto>.Conflict(
                    "Username already exists.");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            User user = new User
            {
                Name = dto.Name,
                Username = dto.Username,
                PasswordHash = passwordHash,
                Role = dto.Role,
                IsActive = dto.IsActive
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            UserDto userDto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Username = user.Username,
                Role = user.Role,
                IsActive = user.IsActive
            };

            return ServiceResult<UserDto>.Success(userDto);
        }
        public async Task<ServiceResult<bool>> ChangeCredentialsAsync(
         int id,
         ChangeCredentialsDto dto)
        {
            User? user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return ServiceResult<bool>.NotFound(
                    "User not found.");
            }

            bool usernameExists = await _context.Users
                .AnyAsync(u =>
                    u.Id != id &&
                    u.Username == dto.Username);

            if (usernameExists)
            {
                return ServiceResult<bool>.Conflict(
                    "Username already exists.");
            }

            user.Username = dto.Username;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(
                dto.Password);

            await _context.SaveChangesAsync();

            return ServiceResult<bool>.Success(true);
        }

        public async Task<ServiceResult<bool>> DeleteUserAsync(int id)
        {
            User? user = await _context.Users
                .FindAsync(id);

            if (user == null)
            {
                return ServiceResult<bool>.NotFound(
                    "User not found.");
            }

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            return ServiceResult<bool>.Success(true);
        }
        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            List<UserDto> users = await _context.Users
                .AsNoTracking()
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Username = u.Username,
                    Role = u.Role,
                    IsActive = u.IsActive
                })
                .ToListAsync();

            return users;
        }

        public async Task<ServiceResult<UserDto>> GetUserByIdAsync(int id)
        {
            User? user = await _context.Users
                .FindAsync(id);

            if (user == null)
            {
                return ServiceResult<UserDto>.NotFound(
                    "User not found.");
            }

            UserDto userDto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Username = user.Username,
                Role = user.Role,
                IsActive = user.IsActive
            };

            return ServiceResult<UserDto>.Success(userDto);
        }
        public async Task<ServiceResult<UserDto>> UpdateUserAsync(
         int id,
         UpdateUserDto dto)
        {
            User? user = await _context.Users
                .FindAsync(id);

            if (user == null)
            {
                return ServiceResult<UserDto>.NotFound(
                    "User not found.");
            }

            user.Name = dto.Name;
            user.Role = dto.Role;
            user.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();

            UserDto userDto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Username = user.Username,
                Role = user.Role,
                IsActive = user.IsActive
            };

            return ServiceResult<UserDto>.Success(userDto);
        }
    }
}
