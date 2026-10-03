using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoffeeHub.API.DTOs.Orders
{
    public class AddOrderItemDto
    {
        [Required]
        public int ProductId { get; set; }

        [DefaultValue(1)]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;
    }
}
