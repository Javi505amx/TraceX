namespace TraceX.Application.DTOs.Users
{
    public class UserDto
    {
        public int Id { get; set; }
        public string EmployeeNumber { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Role { get; set; } = default!;
        public bool IsActive { get; set; }

    }
}
