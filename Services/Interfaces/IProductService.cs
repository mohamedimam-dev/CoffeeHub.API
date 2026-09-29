using CoffeeHub.API.Common;
using CoffeeHub.API.DTOs.Products;

namespace CoffeeHub.API.Services.Interfaces
{
    public interface IProductService
    {
        Task<ServiceResult<ProductDto>> AddProductAsync(AddProductDto dto);

        Task<ServiceResult<ProductDto>> GetProductByIdAsync(int id);

        Task<List<ProductDto>> GetAllProductsAsync();

        Task<ServiceResult<ProductDto>> UpdateProductAsync(int id, UpdateProductDto dto);

        Task<ServiceResult<bool>> DeleteProductAsync(int id);
    }
}
