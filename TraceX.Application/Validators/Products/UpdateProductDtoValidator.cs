using FluentValidation;
using TraceX.Application.DTOs.Products;

namespace TraceX.Application.Validators.Products
{
    public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductDtoValidator()
        {
            RuleFor(p => p.Name)
               .NotEmpty().WithMessage("Product Name is required.")
               .MinimumLength(3).WithMessage("Product Name must be at least 3 characters.")
               .MaximumLength(100).WithMessage("Product Name cannot exceed 100 characters.")
               .Matches(@"^[A-Za-z0-9 _()/.-]+$")
               .WithMessage("Product Name can only contain letters, numbers, spaces, hyphens (-), underscores (_), parentheses (), slashes (/), and periods (.).");

            RuleFor(p => p.Description)
               .MaximumLength(250)
               .WithMessage("Description cannot exceed 250 characters.")
               .When(p => !string.IsNullOrWhiteSpace(p.Description));
        }
    }
}
