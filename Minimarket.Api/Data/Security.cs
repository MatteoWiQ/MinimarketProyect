using System;
using System.Collections.Generic;

namespace Minimarket.Api.Data;

public partial class Security
{
    public int Id { get; set; }

    public string Password { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Role { get; set; } = null!;
}
