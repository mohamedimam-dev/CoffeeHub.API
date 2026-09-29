using System.ComponentModel.DataAnnotations;

namespace CoffeeHub.API.DTOs.Users
{
    public class AddUserDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = null!;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = null!;

        [Range(1, 2)]
        public byte Role { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
