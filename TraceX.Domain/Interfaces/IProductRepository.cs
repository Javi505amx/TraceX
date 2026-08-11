using TraceX.Domain.Entities;

namespace TraceX.Domain.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<Product>> GetAllActiveProductsAsync(CancellationToken cancellationToken = default);
    Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Product?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default);
    Task<bool> IsActiveAsync(int id, CancellationToken cancellationToken = default);
    Task<int> UpdateAsync(Product product, CancellationToken cancellationToken = default);
    Task<int> DeleteAsync(int id, CancellationToken cancellationToken = default);
}