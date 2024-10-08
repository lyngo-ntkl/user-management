﻿using System.ComponentModel.DataAnnotations;
using UserManagement.Application.Common;

namespace UserManagement.Application.Dtos.Requests
{
    public class UserRegistrationRequestDto
    {
        [EmailAddress]
        public required string Email { get; set; }
        [Password]
        public required string Password { get; set; }
        public required string UserName { get; set; }
    }
}
