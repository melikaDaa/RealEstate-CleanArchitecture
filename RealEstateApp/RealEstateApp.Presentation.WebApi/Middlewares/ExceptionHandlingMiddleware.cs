using RealEstateApp.Presentation.WebApi.Exceptions;
using System.Net;
using System.Text.Json;

namespace RealEstateApp.Presentation.WebApi.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            int statusCode = (int)HttpStatusCode.InternalServerError;
            string message = "خطای داخلی سرور رخ داده است.";

            if (exception is KeyNotFoundException)
            {
                statusCode = (int)HttpStatusCode.NotFound;
                message = "موردی با این شناسه یافت نشد.";
            }

            var response = new { statusCode, message };
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(response);
        }


    }
}
