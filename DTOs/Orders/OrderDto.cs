namespace CoffeeHub.API.DTOs.Orders
{
    public class OrderDto
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public decimal TotalAmount { get; set; }

        public byte PaymentMethod { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<OrderItemDto> Items { get; set; } = new();
    }
}
