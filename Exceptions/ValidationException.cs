using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoviIDApiCore.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException() : base()
        {
        }

        public ValidationException(string message) : base (message)
        {
        }

        public ValidationException(Exception inner, string message) : base(message, inner)
        {
        }
    }
}
