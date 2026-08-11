using Microsoft.EntityFrameworkCore;
using TraceX.Domain.Entities;
using TraceX.Domain.Interfaces;
using TraceX.Infrastructure.Data; // Ajusta a tu namespace real de DbContext

namespace TraceX.Infrastructure.Repositories;

public class MachineRepository : IMachineRepository
{
    private readonly TraceXDbContext _context;

    public MachineRepository(TraceXDbContext context)
    {
        _context = context;
    }

    public async Task<List<Machine>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Machines
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Machine?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Machines.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<Machine?> GetBySerialNumberAsync(string serialNumber, CancellationToken cancellationToken = default)
    {
        return await _context.Machines
            .FirstOrDefaultAsync(m => m.SerialNumber == serialNumber, cancellationToken);
    }

    public async Task<Machine> AddAsync(Machine machine, CancellationToken cancellationToken = default)
    {
        await _context.Machines.AddAsync(machine, cancellationToken);
        return machine;
    }

    public Task UpdateAsync(Machine machine, CancellationToken cancellationToken = default)
    {
        _context.Machines.Update(machine);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var machine = await _context.Machines.FindAsync(new object[] { id }, cancellationToken);
        if (machine != null)
        {
            _context.Machines.Remove(machine);
        }
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}