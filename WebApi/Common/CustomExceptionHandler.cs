using Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Common;

public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        var (title, detail, statusCode) = GetExceptionDetails(exception);
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        logger.LogError(exception, "❌ {Exception}: {Message}", exception.GetType().Name, exception.Message);

        var problemDetails = new ProblemDetails
        {
            Title = title,
            Detail = detail,
            Status = statusCode,
            Instance = context.Request.Path
        };

        problemDetails.Extensions["traceId"] = context.TraceIdentifier;

        if (exception is ValidationException validationEx)
        {
            problemDetails.Extensions["validationErrors"] = validationEx.Errors;
        }

        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);
        return true;
    }

    private static (string Title, string Detail, int StatusCode) GetExceptionDetails(Exception exception) =>
        exception switch
        {
            ValidationException ex => ("Validation Error", ex.Message, StatusCodes.Status400BadRequest),
            NotFoundException ex => ("Not Found", ex.Message, StatusCodes.Status404NotFound),
            BadRequestException ex => ("Bad Request", ex.Message, StatusCodes.Status400BadRequest),
            UnauthorizedAccessException => ("Unauthorized", "You are not authorized.", StatusCodes.Status401Unauthorized),
            ForbiddenAccessException => ("Forbidden", "You are forbidden from accessing this resource.", StatusCodes.Status403Forbidden),
            InternalServerException ex => ("Server Error", ex.Message, StatusCodes.Status500InternalServerError),
            _ => ("Server Error", exception.Message, StatusCodes.Status500InternalServerError)
        };
}
