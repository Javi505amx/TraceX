using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraceX.Application.DTOs.Users
{
    public  class CreateUserDto
    {

        public required string EmployeeNumber { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        public required string Email { get; set; }

        public required string Role { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
