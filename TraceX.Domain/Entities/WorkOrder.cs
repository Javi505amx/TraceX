using TraceX.Domain.Common;

namespace TraceX.Domain.Entities
{

    public enum WorkOrderStatus
    {
        Created = 1,
        InProgress = 2,
        Paused = 3,
        Completed = 4,
        Cancelled = 5
    }
    public class WorkOrder : BaseEntity
    {
        public required string OrderNumber { get; set; }

        public int TargetQuantity { get; set; }

        public int CompletedQuantity { get; set; }

        public WorkOrderStatus Status { get; set; } = WorkOrderStatus.Created;


        // Relationships
        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;

        public int? MachineId { get; set; }
        public Machine? Machine { get; set; }
    }
}
