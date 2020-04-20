using System.Net;

namespace CoviIDApiCore.V1.DTOs.System
{
    public class Response : BaseResponse
    {
        public object Data { get; set; }

        public Response() : base()
        {
        }

        public Response(bool success, HttpStatusCode code, string message = null) : base(success, code, message)
        {
            SetMetaData(code);
        }
        public Response(object data, HttpStatusCode code, bool success = false, string message = null) : base(success, code, message)
        {
            SetMetaData(code);
            Data = data;
        }

        public Response(object data, bool success, HttpStatusCode code, string message = null) : base(success, code, message)
        {
            SetMetaData(code);
            Data = data;
        }

        private void SetMetaData(HttpStatusCode code)
        {
            if (code == HttpStatusCode.OK)
            {
                Meta.Success = true;
                Meta.Message = "Succesfully executed the task.";
            }
        }
    }
}
