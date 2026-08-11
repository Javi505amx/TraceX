using FluentValidation;
using TraceX.Application.DTOs.Users;

namespace TraceX.Application.Validators.Users
{
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        private static readonly string[] AllowedRoles = ["Operator", "QualityInspector", "Admin"];

        public UpdateUserDtoValidator()
        {

            RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 chars");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name its required")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 chars.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email address is required")
                .EmailAddress().WithMessage("Email address its not valid")
                .MaximumLength(100).WithMessage("Email address cannot exceed 100 chars.");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required")
                .Must(role => AllowedRoles.Contains(role))
                .WithMessage($"Not valid Role, Allowed roles: {string.Join(", ", AllowedRoles)}.");
        }
    }
}
