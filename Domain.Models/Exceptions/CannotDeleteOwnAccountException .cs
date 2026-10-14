using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models.Exceptions
{
    public sealed class CannotDeleteOwnAccountException : Exception
    {
        public string Title { get; } = "Cannot delete own account";
        public CannotDeleteOwnAccountException(string message)
            : base(message)
        {

        }
    }
}
