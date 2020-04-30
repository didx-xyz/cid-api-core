using System.Net;

namespace CoviIDApiCore.V1.DTOs.System
{
    public class Meta
    {
        public bool Success { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }

        public Meta()
        {
        }

        public Meta(bool success, HttpStatusCode code, string message = null)
        {
            Success = success;
            Code = (int)code;
            Message = message;
        }
    }
}
