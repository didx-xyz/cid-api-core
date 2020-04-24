using System;

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
