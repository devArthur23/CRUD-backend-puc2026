using ProductsApi.Models;

namespace ProductsApi.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync(string? category = null, bool? isActive = null);
    Task<Product?> GetByIdAsync(int id);
    Task<Product> CreateAsync(Product product);
    Task<Product?> UpdateAsync(int id, Product product);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
