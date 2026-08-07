using Microsoft.EntityFrameworkCore;
using TraceX.Domain.Entities;
using TraceX.Domain.Interfaces;
using TraceX.Infrastructure.Data;

namespace TraceX.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public readonly TraceXDbContext _context;

        public ProductRepository(TraceXDbContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .ToListAsync();

        }

        public async Task<List<Product>> GetAllActiveProductsAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.IsActive)
                .ToListAsync();
        }
        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
            // El método FindAsync de EF Core solo busca por la clave primaria (el Id).
            //Si le pasas un string name, arrojará una excepción
        }

        public async Task<Product?> GetByNameAsync(string name)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Name == name);
        }


        public async Task<Product> AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> IsActiveAsync(int id)
        {
            return await _context.Products
                .AnyAsync(p => p.Id == id && p.IsActive);
        }

        public async Task<int> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product is null) return 0;

            _context.Products.Remove(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            return await _context.SaveChangesAsync();
        }
    }
}
