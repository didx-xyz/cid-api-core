using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;
using System.Text;
using CoviIDApiCore.V1.DTOs.System;

namespace CoviIDApiCore.Middleware
{
    public class CustomBadRequestMiddleware : ValidationProblemDetails
    {
        public Meta Meta { get; set; }
        public object Data { get; set; }

        public CustomBadRequestMiddleware(ActionContext context)
        {
            Meta = new Meta
            {
                Code = (int)HttpStatusCode.BadRequest,
                Message = ConstructErrorMessage(context),
                Success = false
            };
        }

        private string ConstructErrorMessage(ActionContext context)
        {
            StringBuilder errorString = new StringBuilder();

            foreach (var keyModelStatePair in context.ModelState)
            {
                var errors = keyModelStatePair.Value.Errors;
                if (errors != null && errors.Count > 0)
                {
                    foreach (var error in errors)
                    {
                        errorString.Append(GetErrorMessage(error) + ", ");
                    }
                }
            }

            errorString.Remove(errorString.Length - 2, 2);
            return errorString.ToString();
        }

        private string GetErrorMessage(ModelError error)
        {
            return string.IsNullOrEmpty(error.ErrorMessage) ?
            "The input was not valid." :
            error.ErrorMessage;
        }
    }
}
