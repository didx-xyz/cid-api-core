using System;
using System.Runtime.Serialization;

namespace CoviIDApiCore.Exceptions
{
    [Serializable]
    internal class ClickatellException : Exception
    {
        public ClickatellException()
        {
        }

        public ClickatellException(string message) : base(message)
        {
        }

        public ClickatellException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ClickatellException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}