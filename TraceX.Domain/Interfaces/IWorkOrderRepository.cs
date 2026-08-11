using TraceX.Domain.Entities;

namespace TraceX.Domain.Interfaces;

public interface IWorkOrderRepository
{
    Task<WorkOrder?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<WorkOrder?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<WorkOrder>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<WorkOrder> AddAsync(WorkOrder workOrder, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    void Update(WorkOrder workOrder);
    void Delete(WorkOrder workOrder);
}