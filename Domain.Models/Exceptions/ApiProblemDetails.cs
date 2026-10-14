using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models.Exceptions
{
    public sealed class ApiProblemDetails:ProblemDetails
    {
        public string? ErrorCode { get; set; }

    }
}
