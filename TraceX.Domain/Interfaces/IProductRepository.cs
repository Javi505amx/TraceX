using TraceX.Domain.Entities;

namespace TraceX.Domain.Interfaces
{
    public interface IProductRepository
    {
        // Get an Async List for all objects in <T>
        Task<List<Product>> GetAllAsync();
        Task<List<Product>> GetAllActiveProductsAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product?> GetByNameAsync(string name);
        Task<bool> IsActiveAsync(int id);


        Task<Product> AddAsync(Product product);
        Task<int> UpdateAsync(Product product);
        Task<int> DeleteAsync(int id);

    }
}
