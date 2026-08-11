using Microsoft.EntityFrameworkCore;
using TraceX.Domain.Entities;
using TraceX.Domain.Interfaces;
using TraceX.Infrastructure.Data;

namespace TraceX.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly TraceXDbContext _context;

        public UserRepository(TraceXDbContext context)
        {
            _context = context;

        }

        public async Task<User> AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is null) return 0;

            _context.Users.Remove(user);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<User?> GetByEmployeeNumberAsync(string employeeNumber)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.EmployeeNumber == employeeNumber);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<int> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .AnyAsync(u => u.Id == id, cancellationToken);
        }

        public async Task<bool> ExistsByEmployeeNumberAsync(string employeeNumber, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .AnyAsync(u => u.EmployeeNumber == employeeNumber, cancellationToken);
        }

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .AnyAsync(u => u.Email == email, cancellationToken);
        }
    }
}
