using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Shared.Kernel.Exceptions;
using System.Net.Mime;
using System.Text.Json;

namespace BKCordServer.WebAPI.Middlewares;

public class ExceptionMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException exception)
        {
            var problemDetails = new ProblemDetails
            {
                Type = "ValidationFailure",
                Title = "Validation error",
                Status = StatusCodes.Status400BadRequest,
                Detail = "One or more validation errors has occurred"
            };

            if (exception.Errors is not null)
            {
                problemDetails.Extensions["errors"] = exception.Errors;
            }

            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;
        object errorResponse;

        if (ex is BaseException baseEx)
        {
            context.Response.StatusCode = baseEx.StatusCode;
            errorResponse = new ErrorResult
            {
                Message = baseEx.Message,
                StatusCode = baseEx.StatusCode
            };
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            errorResponse = new ErrorResult
            {
                Message = ex.Message,
                StatusCode = context.Response.StatusCode
            };
        }

        _logger.LogError(ex, ex.Message);

        return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }
}
