using CoffeeHub.API.DTOs.Products;

namespace CoffeeHub.API.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> AddProductAsync(AddProductDto dto);

        Task<ProductDto?> GetProductByIdAsync(int id);

        Task<List<ProductDto>> GetAllProductsAsync();

        Task<ProductDto?> UpdateProductAsync(int id, UpdateProductDto dto);

        Task<bool> DeleteProductAsync(int id);
    }
}
