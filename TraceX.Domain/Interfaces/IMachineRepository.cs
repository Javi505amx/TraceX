using TraceX.Domain.Entities;

namespace TraceX.Domain.Interfaces;

public interface IMachineRepository
{
    Task<List<Machine>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Machine?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Machine?> GetBySerialNumberAsync(string serialNumber, CancellationToken cancellationToken = default);
    Task<Machine> AddAsync(Machine machine, CancellationToken cancellationToken = default);
    Task UpdateAsync(Machine machine, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}