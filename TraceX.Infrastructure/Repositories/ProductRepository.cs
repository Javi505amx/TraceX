using Microsoft.EntityFrameworkCore;
using TraceX.Domain.Entities;
using TraceX.Domain.Interfaces;
using TraceX.Infrastructure.Data;

namespace TraceX.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly TraceXDbContext _context;

    public ProductRepository(TraceXDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Products
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Product>> GetAllActiveProductsAsync(CancellationToken cancellationToken)
    {
        return await _context.Products
            .AsNoTracking()
            .Where(p => p.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Products.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<Product?> GetByNameAsync(string name, CancellationToken cancellationToken) // Corregido typo 'cancellationTokeb'
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Name == name, cancellationToken);
    }

    public async Task<Product> AddAsync(Product product, CancellationToken cancellationToken)
    {
        await _context.Products.AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return product;
    }

    public async Task<bool> IsActiveAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Products
            .AnyAsync(p => p.Id == id && p.IsActive, cancellationToken);
    }

    public async Task<int> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FindAsync(new object[] { id }, cancellationToken);
        if (product is null) return 0;

        _context.Products.Remove(product);
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> UpdateAsync(Product product, CancellationToken cancellationToken)
    {
        _context.Products.Update(product);
        return await _context.SaveChangesAsync(cancellationToken);
    }
}