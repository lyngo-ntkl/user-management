using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.Exceptions;

namespace UserManagement.API.Common
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

            var problemDetails = new ProblemDetails
            {
                Detail = exception.Message,
            };

            if (exception is BadRequestException)
            {
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Bad request";
            }
            else if (exception is NotFoundException)
            {
                problemDetails.Status = StatusCodes.Status404NotFound;
                problemDetails.Title = "Not found";
            }
            else if (exception is UnauthorizedException)
            {
                problemDetails.Status = StatusCodes.Status401Unauthorized;
                problemDetails.Title = "Unauthorized";
            }
            else
            {
                problemDetails.Status = StatusCodes.Status500InternalServerError;
                problemDetails.Title = "Internal server error";
            }

            httpContext.Response.StatusCode = problemDetails.Status.Value;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }
    }
}
