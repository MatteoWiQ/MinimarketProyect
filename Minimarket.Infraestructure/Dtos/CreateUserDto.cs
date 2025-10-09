using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Core.Dtos
{
    public class CreateUserDto
    {
        public string UserType { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Telephone { get; set; }
    }

}
