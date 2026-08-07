using FluentValidation;
using TraceX.Application.DTOs.Machines;

namespace TraceX.Application.Validators.Machines
{
    public class CreateMachineDtoValidator : AbstractValidator<CreateMachineDto>
    {
        public CreateMachineDtoValidator()
        {
            RuleFor(x => x.SerialNumber)
                .NotEmpty().WithMessage("Serial Number is required.")
                .MinimumLength(6).WithMessage("Serial Number must be at least 6 characters.")
                .MaximumLength(20).WithMessage("Serial Number cannot exceed 20 characters.")
                .Matches(@"^[A-Za-z0-9_-]+$")
                .WithMessage("Serial Number can only contain letters, numbers, hyphens (-), and underscores (_).");
            //.Matches(@"^[a-zA-Z0-9 _-]+$")
            //.Matches(@"^[a-zA-Z0-9]+$")
            // @"^[A-Za-z0-9 _-]+$"
            // @"^[\w -]+$"

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
