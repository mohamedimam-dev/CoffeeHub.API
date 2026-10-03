using CoffeeHub.API.Common;
using CoffeeHub.API.DTOs.Users;
using CoffeeHub.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("All", Name = "GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers()
        {
            List<UserDto> users =
                await _userService.GetAllUsersAsync();

            return Ok(users);
        }

        [HttpGet("{id}", Name = "GetUserById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            ServiceResult<UserDto> result =
                await _userService.GetUserByIdAsync(id);

            switch (result.Status)
            {
                case ServiceResultStatus.NotFound:
                    return NotFound(result.Message);

                case ServiceResultStatus.Success:
                    return Ok(result.Data);

                default:
                    return StatusCode(
                        StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("Add", Name = "AddUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<UserDto>> AddUser(AddUserDto dto)
        {
            ServiceResult<UserDto> result =
                await _userService.AddUserAsync(dto);

            switch (result.Status)
            {
                case ServiceResultStatus.Conflict:
                    return Conflict(result.Message);

                case ServiceResultStatus.Success:
                    return CreatedAtRoute(
                        "GetUserById",
                        new { id = result.Data!.Id },
                        result.Data);

                default:
                    return StatusCode(
                        StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}", Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> UpdateUser(
         int id,
         UpdateUserDto dto)
        {
            ServiceResult<UserDto> result =
                await _userService.UpdateUserAsync(id, dto);

            switch (result.Status)
            {
                case ServiceResultStatus.NotFound:
                    return NotFound(result.Message);

                case ServiceResultStatus.Success:
                    return Ok(result.Data);

                default:
                    return StatusCode(
                        StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}/Credentials", Name = "ChangeUserCredentials")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ChangeUserCredentials(
         int id,
         ChangeCredentialsDto dto)
        {
            ServiceResult<bool> result =
                await _userService.ChangeCredentialsAsync(id, dto);

            switch (result.Status)
            {
                case ServiceResultStatus.NotFound:
                    return NotFound(result.Message);

                case ServiceResultStatus.Conflict:
                    return Conflict(result.Message);

                case ServiceResultStatus.Success:
                    return NoContent();

                default:
                    return StatusCode(
                        StatusCodes.Status500InternalServerError);
            }
        }
    }
}
