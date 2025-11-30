using Minimarket.Core.Enum;
using System;
using System.Collections.Generic;

namespace Minimarket.Infrastructure.Data;

public partial class Security
{
    public int Id { get; set; }

    public string Password { get; set; } = null!;

    public string Name { get; set; } = null!;
    public RoleType? Role { get; set; }

}
