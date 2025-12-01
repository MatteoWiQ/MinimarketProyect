using Minimarket.Core.Entities;
using Minimarket.Core.Enum;
using System;
using System.Collections.Generic;

namespace Minimarket.Core.Entities;

public partial class Security : BaseEntity
{
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;

    public string Name { get; set; } = null!;
    public RoleType? Role { get; set; }

}
