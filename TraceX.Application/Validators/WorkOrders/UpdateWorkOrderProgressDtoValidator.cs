using FluentValidation;
using TraceX.Application.DTOs.WorkOrders;

namespace TraceX.Application.Validators.WorkOrders;

public class UpdateWorkOrderProgressDtoValidator : AbstractValidator<UpdateWorkOrderProgressDto>
{
    public UpdateWorkOrderProgressDtoValidator()
    {
        // Al menos uno de los dos campos debe estar presente
        RuleFor(x => x)
            .Must(x => x.Status.HasValue || x.CompletedQuantity.HasValue)
            .WithMessage("You must provide at least one field to update (Status o CompletedQuantity).");

        When(x => x.CompletedQuantity.HasValue, () =>
        {
            RuleFor(x => x.CompletedQuantity!.Value)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Completed quantity should not be lower tan 0");
        });

        When(x => x.Status.HasValue, () =>
        {
            RuleFor(x => x.Status!.Value)
                .IsInEnum()
                .WithMessage("Status not valid");
        });
    }
}