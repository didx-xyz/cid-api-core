using System.Net;

namespace CoviIDApiCore.V1.DTOs.System
{
    public abstract class BaseResponse
    {
        public Meta Meta { get; set; }

        public BaseResponse()
        {
        }

        public BaseResponse(bool success, HttpStatusCode code, string message = null)
        {
            Meta = new Meta(success, code, message);
        }
    }
}
