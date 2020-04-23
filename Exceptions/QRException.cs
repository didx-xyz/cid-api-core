using System;
using System.Runtime.Serialization;

namespace CoviIDApiCore.Exceptions
{
    [Serializable]
    internal class QRException : Exception
    {
        public QRException()
        {
        }

        public QRException(string message) : base(message)
        {
        }

        public QRException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected QRException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}