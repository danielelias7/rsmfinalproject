namespace RSMFinalProjectAPI.Middleware
{
    using RSMFinalProjectAPI.Application.Exceptions;

    using System.Net;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(ex, context);
            }
        }

        private async Task HandleExceptionAsync(Exception ex, HttpContext context)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string details = string.Empty;
            switch (ex)
            {
                case BadRequestException badRequestException:
                    statusCode = HttpStatusCode.BadRequest;
                    break;
                case NotFoundException NotFound:
                    statusCode = HttpStatusCode.NotFound;
                    break;
                default:
                    details = ex.StackTrace ?? "";
                    break;

            }
            var response = new { message = ex.Message, statusCode, details };
            
            var jsonResponse = JsonSerializer.Serialize(response);
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
