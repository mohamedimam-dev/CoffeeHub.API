using CoffeeHub.API.Common;
using CoffeeHub.API.Data;
using CoffeeHub.API.DTOs.Products;
using CoffeeHub.API.Entities;
using CoffeeHub.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoffeeHub.API.Services
{
    public class ProductService : IProductService
    {
        private readonly CoffeeHubDbContext _context;

        public ProductService(CoffeeHubDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<ProductDto>> AddProductAsync(AddProductDto dto)
        {
            // 1. Validate input
            if (dto == null)
                return ServiceResult<ProductDto>.BadRequest("Product data is required.");

            // 2. Check duplicate product name
            bool nameExists = await _context.Products
                .AnyAsync(p => p.Name == dto.Name);

            if (nameExists)
                return ServiceResult<ProductDto>.Conflict("A product with the same name already exists.");

            // 3. Create product
            Product product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl,
                IsAvailable = dto.IsAvailable
            };

            // 4. Save
            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            // 5. Map to DTO
            ProductDto productDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                IsAvailable = product.IsAvailable
            };

            // 6. Return success
            return ServiceResult<ProductDto>.Success(productDto);
        }

        public async Task<ServiceResult<bool>> DeleteProductAsync(int id)
        {
            Product? product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return ServiceResult<bool>.NotFound("Product not found.");
            }

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            return ServiceResult<bool>.Success(true);
        }

        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            List<ProductDto> products = await _context.Products
                .AsNoTracking()
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    IsAvailable = p.IsAvailable
                })
                .ToListAsync();

            return products;
        }

        public async Task<ServiceResult<ProductDto>> GetProductByIdAsync(int id)
        {
            Product? product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return ServiceResult<ProductDto>.NotFound("Product not found.");
            }

            ProductDto productDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                IsAvailable = product.IsAvailable
            };

            return ServiceResult<ProductDto>.Success(productDto);
        }
        public async Task<ServiceResult<ProductDto>> UpdateProductAsync(
         int id,
         UpdateProductDto dto)
        {
            Product? product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return ServiceResult<ProductDto>.NotFound("Product not found.");
            }

            bool productExists = await _context.Products
                .AnyAsync(p =>
                    p.Id != id &&
                    p.Name == dto.Name);

            if (productExists)
            {
                return ServiceResult<ProductDto>.Conflict(
                    "Product name already exists.");
            }

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.ImageUrl = dto.ImageUrl;
            product.IsAvailable = dto.IsAvailable;

            await _context.SaveChangesAsync();

            ProductDto productDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                IsAvailable = product.IsAvailable
            };

            return ServiceResult<ProductDto>.Success(productDto);
        }
    }
}
