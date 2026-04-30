using API.DTO.Common;
using API.Web.Common.Exceptions;
using System.Net;
using System.Text.Json;

namespace API.Web.Common
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (statusCode, message, errors) = exception switch
            {
                NotFoundException e => (HttpStatusCode.NotFound, e.Message, null),
                BusinessRuleException e => (HttpStatusCode.UnprocessableEntity, e.Message, null),
                FluentValidation.ValidationException e => (HttpStatusCode.BadRequest, e.Message, FluentExceptionSerializer(e.Errors)),
                _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.", null)
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var payload = JsonSerializer.Serialize(new ApiResponse
            {
                Success = false,
                Message = message,
                Data = errors
            });

            return context.Response.WriteAsync(payload);
        }

        private static List<string> FluentExceptionSerializer(IEnumerable<FluentValidation.Results.ValidationFailure>? errors)
        {
            if (errors == null) return new List<string>();

            return errors.Select(e => e.ErrorMessage).ToList();
        }
    }
}