using FluentValidation;
using TraceX.Application.DTOs.Users;
using TraceX.Domain.Interfaces;

namespace TraceX.Application.Validators.Users
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        private static readonly string[] AllowedRoles = ["Operator", "QualityInspector", "Admin"];
        private readonly IUserRepository _userRepository; // Uncooment this for obtaine the roles form DB Repository
        public CreateUserDtoValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(x => x.EmployeeNumber)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Employee number is required.")
            .MaximumLength(20).WithMessage("Employee number cannot exceed 20 chars.")
            .MustAsync(async (employeeNumber, cancellationToken) =>
                !await _userRepository.ExistsByEmployeeNumberAsync(employeeNumber, cancellationToken))
            .WithMessage("Employee number is already registered.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 chars.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 chars.");

            RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email address is required.")
            .EmailAddress().WithMessage("Email address is not valid.")
            .MaximumLength(100).WithMessage("Email address cannot exceed 100 chars.")
            .MustAsync(async (email, cancellationToken) =>
                !await _userRepository.ExistsByEmailAsync(email, cancellationToken))
            .WithMessage("Email address is already registered.");

            RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Role is required.")
            .Must(role => AllowedRoles.Contains(role))
            .WithMessage($"Invalid role. Allowed roles: {string.Join(", ", AllowedRoles)}.");
        }
    }
}



//using FluentValidation;
//using TraceX.Application.DTOs.Users;
//using TraceX.Domain.Interfaces;

//namespace TraceX.Application.Validators.Users;

//public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
//{
//    private readonly IUserRepository _userRepository;

//    public CreateUserDtoValidator(IUserRepository userRepository)
//    {
//        _userRepository = userRepository;

//        RuleFor(x => x.EmployeeNumber)
//            .NotEmpty().WithMessage("El número de empleado es obligatorio.");

//        RuleFor(x => x.Email)
//            .NotEmpty().WithMessage("El correo es obligatorio.")
//            .EmailAddress().WithMessage("El correo no tiene un formato válido.");

//        // Validación dinámica contra la Base de Datos
//        RuleFor(x => x.Role)
//            .NotEmpty().WithMessage("El rol es obligatorio.")
//            .MustAsync(BeAValidRoleAsync)
//            .WithMessage("El rol especificado no existe en la base de datos.");
//    }

//    private async Task<bool> BeAValidRoleAsync(string role, CancellationToken cancellationToken)
//    {
//        // Consulta asíncrona al repositorio para verificar si el rol existe
//        return await _userRepository.RoleExistsAsync(role, cancellationToken);
//    }
//}