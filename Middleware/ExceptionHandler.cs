using System;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;

using CoviIDApiCore.Exceptions;
using CoviIDApiCore.V1.Constants;
using CoviIDApiCore.V1.DTOs.System;
using CoviIDApiCore.V1.Interfaces.Services;

namespace CoviIDApiCore.Middleware
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly IAuthenticationService _authentication;

        public ExceptionHandler(RequestDelegate next, IAuthenticationService authentication)
        {
            _next = next;
            _authentication = authentication;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                if (!_authentication.IsAuthorized(context.Request.Headers["x-api-key"]))
                    throw new UnauthorizedAccessException();

                await _next(context);
            }
            catch (ValidationException e)
            {
                await HandleValidationException(context, e);
            }
            catch (NotFoundException)
            {
                await HandleNotFoundException(context);
            }
            catch (StreetCredBrokerException e)
            {
                await HandleStreetCredBrokerException(context, e);
            }
            catch (UnauthorizedAccessException e)
            {
                await HandleUnauthorisedException(context, e);
            }
            catch (Exception e)
            {
                await HandleUnexpectedException(context, e);
            }
        }

        #region Exception Handler Methods

        private static Task HandleValidationException(HttpContext context, Exception e)
        {
            var code = HttpStatusCode.BadRequest;

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            var rsp = new Response(false, code, e.Message);
            return context.Response.WriteAsync(JsonConvert.SerializeObject(rsp));
        }

        private static Task HandleNotFoundException(HttpContext context)
        {
            var code = HttpStatusCode.NotFound;

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            var rsp = new Response(false, HttpStatusCode.NotFound);
            return context.Response.WriteAsync(JsonConvert.SerializeObject(rsp));
        }


        private Task HandleStreetCredBrokerException(HttpContext context, StreetCredBrokerException e)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            var message = Messages.Misc_ThirdParty;

            #if DEBUG
            message = e.Message;
            #endif
            var rsp = new Response(false, statusCode, message);
            return context.Response.WriteAsync(JsonConvert.SerializeObject(rsp));
        }

        private static Task HandleUnexpectedException(HttpContext context, Exception e)
        {
            var code = HttpStatusCode.InternalServerError;

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            var message = Messages.Misc_SomethingWentWrong;

            var rsp = new Response(false, code, message);
            return context.Response.WriteAsync(JsonConvert.SerializeObject(rsp));
        }

        private static Task HandleUnauthorisedException(HttpContext context, Exception e)
        {
            var code = HttpStatusCode.Unauthorized;

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            var message = Messages.Misc_Unauthorized;

            var rsp = new Response(false, code, message);
            return context.Response.WriteAsync(JsonConvert.SerializeObject(rsp));
        }
        #endregion Exception Handler Methods
    }
}
