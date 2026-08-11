using FluentValidation;
using TraceX.Api.DTOs;
using TraceX.Domain.Interfaces;

namespace TraceX.Application.Validators.WorkOrders;

public class CreateWorkOrderDtoValidator : AbstractValidator<CreateWorkOrderDto>
{
    private const string WorkOrderPattern = @"^[IAMEBT]\d{4}-\d{2}(0[1-9]|1[0-2])\d{4}$";
    private readonly IProductRepository _productRepository;
    private readonly IMachineRepository _machineRepository;

    public CreateWorkOrderDtoValidator(
        IProductRepository productRepository,
        IMachineRepository machineRepository)
    {
        _productRepository = productRepository;
        _machineRepository = machineRepository;

        RuleFor(w => w.OrderNumber)
            .NotEmpty().WithMessage("Order Number is required.")
            .Matches(WorkOrderPattern)
            .WithMessage("Invalid Order Number format. Expected format: 'M5107-26050009' (Center + Type - YYMMXXXX).");

        RuleFor(w => w.TargetQuantity)
            .GreaterThan(0).WithMessage("Target quantity must be greater than 0.");

        RuleFor(w => w.ProductId)
            .GreaterThan(0).WithMessage("Product Id is required.")
            .MustAsync(async (productId, cancellationToken) =>
                await _productRepository.GetByIdAsync(productId, cancellationToken) != null)
            .WithMessage("Product Id does not exist.");

        When(w => w.MachineId.HasValue, () =>
        {
            RuleFor(w => w.MachineId)
                .MustAsync(async (machineId, cancellationToken) =>
                    await _machineRepository.GetByIdAsync(machineId!.Value, cancellationToken) != null)
                .WithMessage("Machine Id does not exist.");
        });
    }
}