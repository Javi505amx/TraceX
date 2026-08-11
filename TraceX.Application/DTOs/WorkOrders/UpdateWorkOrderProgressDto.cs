using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraceX.Domain.Entities;

namespace TraceX.Application.DTOs.WorkOrders
{
    public record UpdateWorkOrderProgressDto(
    WorkOrderStatus? Status,
    int? CompletedQuantity
);
}
