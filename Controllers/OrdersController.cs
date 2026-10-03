using CoffeeHub.API.Common;
using CoffeeHub.API.DTOs.Orders;
using CoffeeHub.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("All", Name = "GetAllOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<OrderDto>>> GetAllOrders()
        {
            List<OrderDto> orders =
                await _orderService.GetAllOrdersAsync();

            return Ok(orders);
        }

        [HttpGet("{id}", Name = "GetOrderById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDto>> GetOrderById(int id)
        {
            ServiceResult<OrderDto> result =
                await _orderService.GetOrderByIdAsync(id);

            switch (result.Status)
            {
                case ServiceResultStatus.NotFound:
                    return NotFound(result.Message);

                case ServiceResultStatus.Success:
                    return Ok(result.Data);

                default:
                    return StatusCode(
                        StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("Add", Name = "AddOrder")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderDto>> AddOrder(
         AddOrderDto dto,
         int employeeId)
        {
            ServiceResult<OrderDto> result =
                await _orderService.AddOrderAsync(dto, employeeId);

            switch (result.Status)
            {
                case ServiceResultStatus.BadRequest:
                    return BadRequest(result.Message);

                case ServiceResultStatus.Success:
                    return CreatedAtRoute(
                        "GetOrderById",
                        new { id = result.Data!.Id },
                        result.Data);

                default:
                    return StatusCode(
                        StatusCodes.Status500InternalServerError);
            }
        }
    }
}
