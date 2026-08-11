using Microsoft.EntityFrameworkCore;
using TraceX.Domain.Entities;
using TraceX.Domain.Interfaces;
using TraceX.Infrastructure.Data;

namespace TraceX.Infrastructure.Repositories;

public class WorkOrderRepository : IWorkOrderRepository
{
    private readonly TraceXDbContext _context;

    public WorkOrderRepository(TraceXDbContext context)
    {
        _context = context;
    }

    public async Task<WorkOrder?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.WorkOrders
            .Include(w => w.Product)
            .Include(w => w.Machine)
            .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
    }

    public async Task<WorkOrder?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default)
    {
        return await _context.WorkOrders
            .Include(w => w.Product)
            .Include(w => w.Machine)
            .FirstOrDefaultAsync(w => w.OrderNumber == orderNumber, cancellationToken);
    }

    public async Task<IEnumerable<WorkOrder>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.WorkOrders
            .Include(w => w.Product)
            .Include(w => w.Machine)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<WorkOrder> AddAsync(WorkOrder workOrder, CancellationToken cancellationToken)
    {
        await _context.WorkOrders.AddAsync(workOrder);
        await _context.SaveChangesAsync(cancellationToken);
        return workOrder;
    }
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public void Update(WorkOrder workOrder)
    {
        _context.WorkOrders.Update(workOrder);
    }

    public void Delete(WorkOrder workOrder)
    {
        _context.WorkOrders.Remove(workOrder);
    }

    
}