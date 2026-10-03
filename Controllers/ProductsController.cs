using CoffeeHub.API.Common;
using CoffeeHub.API.DTOs.Products;
using CoffeeHub.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("All", Name = "GetAllProducts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ProductDto>>> GetAllProducts()
        {
            List<ProductDto> products = await _productService.GetAllProductsAsync();

            return Ok(products);
        }

        [HttpGet("{id}", Name = "GetProductById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            ServiceResult<ProductDto> result =
                await _productService.GetProductByIdAsync(id);

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

        [HttpPost("Add", Name = "AddProduct")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ProductDto>> AddProduct(AddProductDto dto)
        {
            ServiceResult<ProductDto> result =
                await _productService.AddProductAsync(dto);

            switch (result.Status)
            {
                case ServiceResultStatus.BadRequest:
                    return BadRequest(result.Message);

                case ServiceResultStatus.Conflict:
                    return Conflict(result.Message);

                case ServiceResultStatus.Success:
                    return CreatedAtAction(
                        nameof(GetProductById),
                        new { id = result.Data!.Id },
                        result.Data);

                default:
                    return StatusCode(
                        StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}", Name = "UpdateProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ProductDto>> UpdateProduct(
         int id,
         UpdateProductDto dto)
        {
            ServiceResult<ProductDto> result =
                await _productService.UpdateProductAsync(id, dto);

            switch (result.Status)
            {
                case ServiceResultStatus.NotFound:
                    return NotFound(result.Message);

                case ServiceResultStatus.Conflict:
                    return Conflict(result.Message);

                case ServiceResultStatus.Success:
                    return Ok(result.Data);

                default:
                    return StatusCode(
                        StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}", Name = "DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            ServiceResult<bool> result =
                await _productService.DeleteProductAsync(id);

            switch (result.Status)
            {
                case ServiceResultStatus.NotFound:
                    return NotFound(result.Message);

                case ServiceResultStatus.Success:
                    return NoContent();

                default:
                    return StatusCode(
                        StatusCodes.Status500InternalServerError);
            }
        }
    }
}
