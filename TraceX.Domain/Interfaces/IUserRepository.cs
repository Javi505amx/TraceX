using TraceX.Domain.Entities;

namespace TraceX.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmployeeNumberAsync(string employeeNumber);
        Task<User> AddAsync(User user);
        Task<int> UpdateAsync(User user);
        Task<int> DeleteAsync(int id);

        Task<bool> ExistsByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> ExistsByEmployeeNumberAsync(string employeeNumber, CancellationToken cancellationToken = default);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}
