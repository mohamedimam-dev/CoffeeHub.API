using CoffeeHub.API.Data;
using CoffeeHub.API.DTOs.Orders;
using CoffeeHub.API.Entities;
using CoffeeHub.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoffeeHub.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly CoffeeHubDbContext _context;

        public OrderService(CoffeeHubDbContext context)
        {
            _context = context;
        }

        public async Task<OrderDto> AddOrderAsync(
        AddOrderDto dto,
        int employeeId)
        {
            if (dto.Items == null || dto.Items.Count == 0)
            {
                throw new Exception("Order must contain at least one item.");
            }

            bool employeeExists = await _context.Users
                .AnyAsync(u =>
                    u.Id == employeeId &&
                    u.Role == 1 &&
                    u.IsActive);

            if (!employeeExists)
            {
                throw new Exception("Employee not found or inactive.");
            }

            List<int> productIds = dto.Items
                .Select(i => i.ProductId)
                .Distinct()
                .ToList();

            List<Product> products = await _context.Products
                .Where(p =>
                    productIds.Contains(p.Id) &&
                    p.IsAvailable)
                .ToListAsync();

            if (products.Count != productIds.Count)
            {
                throw new Exception(
                    "One or more products were not found or are unavailable.");
            }

            Order order = new Order
            {
                EmployeeId = employeeId,
                PaymentMethod = dto.PaymentMethod,
                TotalAmount = 0
            };

            foreach (AddOrderItemDto itemDto in dto.Items)
            {
                Product product = products
                    .First(p => p.Id == itemDto.ProductId);

                decimal amount = product.Price * itemDto.Quantity;

                OrderItem orderItem = new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = itemDto.Quantity,
                    UnitPrice = product.Price,
                    Amount = amount
                };

                order.OrderItems.Add(orderItem);

                order.TotalAmount += amount;
            }

            _context.Orders.Add(order);

            await _context.SaveChangesAsync();

            OrderDto orderDto = new OrderDto
            {
                Id = order.Id,
                EmployeeId = order.EmployeeId,
                TotalAmount = order.TotalAmount,
                PaymentMethod = order.PaymentMethod,
                CreatedAt = order.CreatedAt,
                Items = order.OrderItems
                    .Select(item => new OrderItemDto
                    {
                        Id = item.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        Amount = item.Amount
                    })
                    .ToList()
            };

            return orderDto;
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int id)
        {
            Order? order = await _context.Orders
                .AsNoTracking()
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return null;
            }

            OrderDto orderDto = new OrderDto
            {
                Id = order.Id,
                EmployeeId = order.EmployeeId,
                TotalAmount = order.TotalAmount,
                PaymentMethod = order.PaymentMethod,
                CreatedAt = order.CreatedAt,
                Items = order.OrderItems
                    .Select(item => new OrderItemDto
                    {
                        Id = item.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        Amount = item.Amount
                    })
                    .ToList()
            };

            return orderDto;
        }

        public async Task<List<OrderDto>> GetAllOrdersAsync()
        {
            List<OrderDto> orders = await _context.Orders
                .AsNoTracking()
                .Include(o => o.OrderItems)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    EmployeeId = o.EmployeeId,
                    TotalAmount = o.TotalAmount,
                    PaymentMethod = o.PaymentMethod,
                    CreatedAt = o.CreatedAt,
                    Items = o.OrderItems
                        .Select(item => new OrderItemDto
                        {
                            Id = item.Id,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice,
                            Amount = item.Amount
                        })
                        .ToList()
                })
                .ToListAsync();

            return orders;
        }
    }
}
