using System.ComponentModel.DataAnnotations;

namespace TraceX.Domain.Common;

public abstract class BaseEntity
{
    public int Id { get; set; }

    // Auditoría de Creación
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public string? CreatedBy { get; set; }

    // Auditoría de Modificación
    public DateTimeOffset? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }

    // Borrado Lógico (Soft Delete)
    public bool IsDeleted { get; set; } = false;
    public DateTimeOffset? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }

    // Estado del registro
    public bool IsActive { get; set; } = true;

    // Control de Concurrencia Optimista
    [Timestamp]
    public byte[]? RowVersion { get; set; }
}