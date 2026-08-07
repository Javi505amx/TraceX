using TraceX.Domain.Entities;

namespace TraceX.Application.DTOs.Machines
{
    public class MachineDto
    {

        public int Id { get; set; }
        public required string SerialNumber { get; set; }

        public required string ProductionLine { get; set; }

        public MachineStatus Status { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

    }
}
