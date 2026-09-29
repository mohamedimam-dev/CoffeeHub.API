namespace CoffeeHub.API.DTOs.Users
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Username { get; set; } = null!;

        public byte Role { get; set; }

        public bool IsActive { get; set; }
    }
}
