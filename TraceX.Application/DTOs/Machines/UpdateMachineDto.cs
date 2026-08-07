using TraceX.Domain.Entities;

namespace TraceX.Application.DTOs.Machines
{
    public class UpdateMachineDto
    {
        //public int Id { get; set; }
        //public required string SerialNumber { get; set; }

        public required string ProductionLine { get; set; }

        public MachineStatus Status { get; set; }
    }
}
