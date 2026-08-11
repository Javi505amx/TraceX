using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraceX.Domain.Common;

namespace TraceX.Domain.Entities
{
    public class User : BaseEntity
    {
        public required string EmployeeNumber { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        public required string Email { get; set; }

        public required string Role { get; set; }


    }
}
