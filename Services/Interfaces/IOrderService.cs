using CoffeeHub.API.DTOs.Orders;
using CoffeeHub.API.Common;


namespace CoffeeHub.API.Services.Interfaces
{
    public interface IOrderService
    {
        Task<ServiceResult<OrderDto>> AddOrderAsync(
        AddOrderDto dto,
        int employeeId);

        Task<ServiceResult<OrderDto>> GetOrderByIdAsync(int id);

        Task<List<OrderDto>> GetAllOrdersAsync();
    }
}
