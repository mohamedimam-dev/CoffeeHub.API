using System.ComponentModel.DataAnnotations;

namespace CoffeeHub.API.DTOs.Orders
{
    public class AddOrderItemDto
    {
        [Required]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
