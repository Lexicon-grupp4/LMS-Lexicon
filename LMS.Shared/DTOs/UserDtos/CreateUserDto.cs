using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Shared.DTOs.UserDtos
{
    public record CreateUserDto:UserBaseDto
    {
        public string Password { get; set; } = null!;

    }
}
