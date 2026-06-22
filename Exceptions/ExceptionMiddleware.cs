using ClinicFlow.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ClinicFlow.Exceptions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string message = "Something went wrong.";

            switch (exception)
            {
                case ArgumentException:
                    statusCode = HttpStatusCode.NotFound;
                    message = exception.Message;
                    break;
                case DuplicateResourceException:
                    statusCode = HttpStatusCode.Conflict;
                    message = exception.Message;
                    break;
                case InvalidRoleException:
                    statusCode = HttpStatusCode.BadRequest;
                    message = exception.Message;
                    break;
                case InvalidCredentialsException:
                    statusCode = HttpStatusCode.Unauthorized;
                    message = exception.Message;
                    break;
            }

            context.Response.StatusCode = (int)statusCode;

            var response = new ErrorResponse
            {
                TimeStamp = DateTime.UtcNow.ToString("o"),
                Error = message,
                StatusCode = (int)statusCode,
                Path = context.Request.Path
            };

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
