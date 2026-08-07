using FluentValidation;
using TraceX.Application.DTOs.Products;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TraceX.Application.Validators.Products
{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator()
        {
            RuleFor(p => p.PartNumber)
                .NotEmpty().WithMessage("Part Number is required.")
                .MinimumLength(6).WithMessage("Part Number must be at least 6 characters.")
                .MaximumLength(50).WithMessage("Part Number cannot exceed 50 characters.")
                .Matches(@"^[A-Za-z0-9_-]+$")
                .WithMessage("Part Number can only contain letters, numbers, hyphens (-), and underscores (_).");

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
