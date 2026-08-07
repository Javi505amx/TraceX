using TraceX.Domain.Common;

namespace TraceX.Domain.Entities;


public enum MachineStatus
{
    Active = 1,
    Maintenance = 2,
    Offline = 3
}


public class Machine : BaseEntity
{
    //public int Id { get; set; }

    // El 'default' le dice al compilador "confia en mí
    // Entity framework llenará  esto al leer de la BD, no sera nulo".
    public string SerialNumber { get; set; } = default!;

    public required string ProductionLine { get; set; }

    public MachineStatus Status { get; set; }

}