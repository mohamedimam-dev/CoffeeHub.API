using CoffeeHub.API.DTOs.Orders;

namespace CoffeeHub.API.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> AddOrderAsync(
        AddOrderDto dto,
        int employeeId);

        Task<OrderDto?> GetOrderByIdAsync(int id);

        Task<List<OrderDto>> GetAllOrdersAsync();
    }
}
