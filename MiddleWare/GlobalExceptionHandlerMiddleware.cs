using Microsoft.AspNetCore.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace AIIcsoftAPI.MiddleWare
{

    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Title { get; set; }
        public string ExceptionMessage { get; set; }
    }
    public class AppExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<AppExceptionHandler> _logger;
        public AppExceptionHandler(ILogger<AppExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var response = new ErrorResponse()
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ExceptionMessage = exception.Message,
                Title = "Something went wrong"
            };
            _logger.LogError("HttpRequest URL:+" + httpContext.Request.Path.ToString() + " ExceptionMessage:" + exception.Message.ToString() + " StackTrace:" + exception + "InnerException:" +exception.InnerException);

            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
            return true;
        }
    }

    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception switch
            {
                ArgumentNullException => (int)HttpStatusCode.BadRequest,
                ArgumentException => (int)HttpStatusCode.BadRequest,
                ValidationException => (int)HttpStatusCode.BadRequest,
                BadHttpRequestException => (int)HttpStatusCode.BadRequest,
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                _ => (int)HttpStatusCode.InternalServerError,
            };

            string message = context.Response.StatusCode == (int)HttpStatusCode.InternalServerError ? "Something went wrong. Please try again after sometime!" : exception.Message;
            return context.Response.WriteAsync(message);
        }
    }

}
