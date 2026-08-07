using FluentValidation;
using TraceX.Application.DTOs.Machines;

namespace TraceX.Application.Validators.Machines
{
    public class UpdateMachineDtoValidator : AbstractValidator<UpdateMachineDto>
    {
        public UpdateMachineDtoValidator()
        {
            RuleFor(x => x.ProductionLine)
                .NotEmpty().WithMessage("Production Line is required.")
                .MinimumLength(3).WithMessage("Production Line must be at least 3 characters.")
                .MaximumLength(50).WithMessage("Production Line cannot exceed 50 characters.")
                .Matches(@"^[\w -]+$")
                .WithMessage("Production Line can only contain letters, numbers, spaces, hyphens (-), and underscores (_).");

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Not a valid machine status.");
        }
    }
}
