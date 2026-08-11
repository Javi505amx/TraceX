using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TraceX.Api.DTOs;
using TraceX.Application.DTOs.WorkOrders;
using TraceX.Domain.Entities;
using TraceX.Domain.Interfaces;

namespace TraceX.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkOrdersController : ControllerBase
{
    private readonly IWorkOrderRepository _workOrdersRepository;
    private readonly IValidator<CreateWorkOrderDto> _createWorkOrderDtoValidator;

    public WorkOrdersController(IWorkOrderRepository workOrdersRepository, IValidator<CreateWorkOrderDto> createWorkOrderDtoValidator)
    {
        _workOrdersRepository = workOrdersRepository;
        _createWorkOrderDtoValidator = createWorkOrderDtoValidator;

    }

    [HttpGet]
    public async Task<ActionResult<List<WorkOrderDto>>> GetAllWorkOrders(CancellationToken cancellationToken)
    {
        var workOrders = await _workOrdersRepository.GetAllAsync(cancellationToken);
        var workOrderDtos = workOrders.Adapt<List<WorkOrderDto>>();
        return Ok(workOrderDtos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<WorkOrderDto>> GetWorkOrderById(int id, CancellationToken cancellationToken)
    {
        var workOrder = await _workOrdersRepository.GetByIdAsync(id, cancellationToken);
        if (workOrder == null) return NotFound();

        var workOrderDto = workOrder.Adapt<WorkOrderDto>();

        return Ok(workOrderDto);
    }

    [HttpGet("by-number/{orderNumber}")]
    public async Task<ActionResult<WorkOrderDto>> GetByOrderNumber(string orderNumber, CancellationToken cancellationToken)
    {
        var workOrder = await _workOrdersRepository.GetByOrderNumberAsync(orderNumber, cancellationToken);
        if (workOrder == null)
        {
            return NotFound(new { message = $"No se encontró la orden de trabajo '{orderNumber}'." });
        }

        var workorderDto = workOrder.Adapt<WorkOrderDto>();

        return Ok(workorderDto);
    }

    [HttpPost]
    public async Task<ActionResult<WorkOrderDto>> Create(CreateWorkOrderDto dto, CancellationToken cancellationToken)
    {
        var resultValidation = await _createWorkOrderDtoValidator.ValidateAsync(dto, cancellationToken);
        if (!resultValidation.IsValid) return BadRequest(resultValidation.ToDictionary());

        var existingWorkOrder = await _workOrdersRepository.GetByOrderNumberAsync(dto.OrderNumber, cancellationToken);
        if (existingWorkOrder != null)
        {
            return Conflict(new { message = $"Ya existe una orden de trabajo registrada con el número '{dto.OrderNumber}'." });
        }

        var workOrderEntity = dto.Adapt<WorkOrder>();

        await _workOrdersRepository.AddAsync(workOrderEntity, cancellationToken);
        await _workOrdersRepository.SaveChangesAsync(cancellationToken);

        // Re-consultamos con .Include() para que el DTO lleve los nombres de Product y Machine
        var createdWorkOrder = await _workOrdersRepository.GetByIdAsync(workOrderEntity.Id, cancellationToken);

        if (createdWorkOrder == null)
        {
            return StatusCode(500, new { message = "Error al recuperar la orden de trabajo recién creada." });
        }

        var responseDto = createdWorkOrder.Adapt<WorkOrderDto>();

        return CreatedAtAction(nameof(GetWorkOrderById), new { id = responseDto.Id }, responseDto);
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<WorkOrderDto>> UpdateProgress(
    int id,
    [FromBody] UpdateWorkOrderProgressDto dto,
    [FromServices] IValidator<UpdateWorkOrderProgressDto> validator,
    CancellationToken cancellationToken)
    {
        // 1. Validar el DTO de entrada
        var validationResult = await validator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToDictionary());

        // 2. Buscar la entidad existente
        var workOrder = await _workOrdersRepository.GetByIdAsync(id, cancellationToken);
        if (workOrder == null)
            return NotFound(new { message = $"No se encontró la orden de trabajo con ID {id}." });

        // 3. Regla de negocio opcional: Evitar que CompletedQuantity supere el TargetQuantity
        if (dto.CompletedQuantity.HasValue && dto.CompletedQuantity.Value > workOrder.TargetQuantity)
        {
            return BadRequest(new
            {
                message = $"Completed quantity ({dto.CompletedQuantity.Value}) it cannot be greater than the target quantity ({workOrder.TargetQuantity})."
            });
        }

        // 4. Aplicar cambios según los valores recibidos
        if (dto.Status.HasValue)
            workOrder.Status = dto.Status.Value;

        if (dto.CompletedQuantity.HasValue)
            workOrder.CompletedQuantity = dto.CompletedQuantity.Value;

        // 5. Guardar cambios en base de datos
        try
        {
            _workOrdersRepository.Update(workOrder);
            await _workOrdersRepository.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict(new { message = "La orden de trabajo fue modificada por otro proceso o usuario. Por favor, recargue los datos e intente nuevamente." });
        }

        var updatedWorkOrder = await _workOrdersRepository.GetByIdAsync(id, cancellationToken);
        return Ok(updatedWorkOrder.Adapt<WorkOrderDto>());
    }

}