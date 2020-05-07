using System;
using System.Runtime.Serialization;

namespace CoviIDApiCore.Exceptions
{
    [Serializable]
    internal class SendGridException : Exception
    {
        public SendGridException()
        {
        }

        public SendGridException(string message) : base(message)
        {
        }

        public SendGridException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SendGridException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}