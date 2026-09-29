using System.ComponentModel.DataAnnotations;

namespace CoffeeHub.API.DTOs.Users
{
    public class ChangeCredentialsDto
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; } = null!;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = null!;
    }
}
