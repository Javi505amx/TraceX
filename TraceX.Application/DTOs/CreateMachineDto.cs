using TraceX.Domain.Entities;

namespace TraceX.Application.DTOs
{
    public class CreateMachineDto
    {
        public required string SerialNumber { get; set; }

        public string? ProductionLine { get; set; }

        public MachineStatus Status { get; set; }

    }
}
