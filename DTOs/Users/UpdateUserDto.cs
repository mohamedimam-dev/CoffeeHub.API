using System.ComponentModel.DataAnnotations;

namespace CoffeeHub.API.DTOs.Users
{
    public class UpdateUserDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [Range(1, 2)]
        public byte Role { get; set; }

        public bool IsActive { get; set; }
    }
}
