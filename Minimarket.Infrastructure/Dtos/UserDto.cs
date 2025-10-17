﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Core.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserType { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Email { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Telephone { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Password { get; set; }
    }
}