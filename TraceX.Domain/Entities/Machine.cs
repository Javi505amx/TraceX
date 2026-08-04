namespace TraceX.Domain.Entities;


public enum MachineStatus
{
    Active = 1,
    Maintenance = 2,
    Offline = 3
}


public class Machine
{
    public int Id { get; set; }

    // El 'default' le dice al compilador "confia en mí
    // Entity framework llenará  esto al leer de la BD, no sera nulo".
    public string SerialNumber { get; set; } = default!;

    public string? ProductionLine { get; set; } // puede ser nulo, no es obligatorio

    public MachineStatus Status { get; set; }

}