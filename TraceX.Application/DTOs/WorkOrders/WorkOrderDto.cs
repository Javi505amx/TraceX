namespace TraceX.Api.DTOs;

public record WorkOrderDto(
    int Id,
    string OrderNumber,
    int TargetQuantity,
    int CompletedQuantity,
    string Status,
    int ProductId,
    string? ProductName,
    int? MachineId,
    string? MachineSerialNumber,
    DateTimeOffset CreatedAt
);