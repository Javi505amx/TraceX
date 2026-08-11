using System.ComponentModel.DataAnnotations;

namespace TraceX.Api.DTOs;

public record CreateWorkOrderDto(

    string OrderNumber,
    int TargetQuantity,
    int ProductId,
    int? MachineId
);