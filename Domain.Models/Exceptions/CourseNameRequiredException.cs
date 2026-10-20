using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models.Exceptions
{
    public class CourseNameRequiredException : Exception
    {
        public CourseNameRequiredException(string message) : base(message)
        {
        }
    }
}
