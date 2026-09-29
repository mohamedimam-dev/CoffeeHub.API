using System.ComponentModel.DataAnnotations;

namespace CoffeeHub.API.DTOs.Orders
{
    public class AddOrderDto
    {
        [Range(1, 2)]
        public byte PaymentMethod { get; set; }

        [Required]
        [MinLength(1)]
        public List<AddOrderItemDto> Items { get; set; } = new();
    }
}
