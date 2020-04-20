using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CoviIDApiCore.Exceptions;
using CoviIDApiCore.V1.DTOs.System;

namespace CoviIDApiCore.Middleware
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;

        public ExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
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
            var message = "Oops! Something went wrong with a third party.";

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

            var message = "Oops! Something went wrong. Please try again later.";

            // To Do: Use the standard response object
            var rsp = new Response(false, code, message);
            return context.Response.WriteAsync(JsonConvert.SerializeObject(rsp));
        }

        #endregion Exception Handler Methods
    }
}
