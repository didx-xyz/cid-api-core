using System;

namespace CoviIDApiCore.Exceptions
{
    public class DbException : Exception
    {
        public DbException() : base()
        {
        }

        public DbException(string message) : base(message)
        {
        }

        public DbException(Exception inner, string message) : base(message, inner)
        {
        }
    }
}
