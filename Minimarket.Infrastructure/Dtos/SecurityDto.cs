using Minimarket.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Infrastructure.Dtos
{
    public class SecurityDto
    {
        public string Name { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public RoleType? Role { get; set; }
    }

}
