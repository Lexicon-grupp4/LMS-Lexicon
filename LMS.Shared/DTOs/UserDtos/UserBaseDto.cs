using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Shared.DTOs.UserDtos
{
    public abstract record UserBaseDto 
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string Role { get; set; } =null!;
        public int? CourseId { get; set; }
        public string? CourseName { get; set; }

    }
}
