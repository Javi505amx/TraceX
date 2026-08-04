using FluentValidation;
using TraceX.Application.DTOs;

namespace TraceX.Application.Validators
{
    public class UpdateMachineDtoValidator : AbstractValidator<UpdateMachineDto>
    {
        public UpdateMachineDtoValidator()
        {
            RuleFor(x => x.SerialNumber)
               .NotEmpty().WithMessage("Serial number is required")
               .MinimumLength(6).WithMessage("Serial number should be at least 6 chars length")
               .Matches(@"^[a-zA-Z0-9]+$").WithMessage("serial number format not allowed")
               .MaximumLength(20).WithMessage("Serial number cannot exceed 20 chars length");


            RuleFor(x => x.ProductionLine)
              .NotEmpty().WithMessage("Production Line is required")
              .MinimumLength(3).WithMessage("Production Line should be at least 3 chars length")
              .Matches(@"^[a-zA-Z0-9]+$").WithMessage("Production Line  format not allowed")
              .MaximumLength(50).WithMessage("Production Line cannot exceed 50 chars length");
            //  Status
            RuleFor(x => x.Status)
                .NotNull().WithMessage("Status is required")
                .IsInEnum().WithMessage("Not a valid machine status");
        }
    }
}
