using System;
using System.Runtime.Serialization;

namespace CoviIDApiCore.Exceptions
{
    [Serializable]
    internal class StreetCredBrokerException : Exception
    {
        public StreetCredBrokerException()
        {
        }

        public StreetCredBrokerException(string message) : base(message)
        {
        }

        public StreetCredBrokerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected StreetCredBrokerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}