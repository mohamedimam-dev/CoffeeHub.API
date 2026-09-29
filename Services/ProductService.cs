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

        public async Task<ProductDto> AddProductAsync(AddProductDto dto)
        {
            Product product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl,
                IsAvailable = dto.IsAvailable
            };

            _context.Products.Add(product);

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

            return productDto;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            Product? product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return false;
            }

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            return true;
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

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            Product? product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return null;
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

            return productDto;
        }

        public async Task<ProductDto?> UpdateProductAsync(
         int id,
         UpdateProductDto dto)
        {
            Product? product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return null;
            }

            bool productExists = await _context.Products
                .AnyAsync(p =>
                    p.Id != id &&
                    p.Name == dto.Name);

            if (productExists)
            {
                throw new Exception("Product name already exists.");
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

            return productDto;
        }
    }
}
