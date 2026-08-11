using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace TraceX.Api.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger,
        IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Excepción no controlada interceptada: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/problem+json";

        var problemDetails = new ProblemDetails
        {
            Instance = context.Request.Path
        };

        switch (exception)
        {
            case KeyNotFoundException:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                problemDetails.Status = (int)HttpStatusCode.NotFound;
                problemDetails.Title = "Recurso no encontrado";
                problemDetails.Detail = exception.Message;
                problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4";
                break;

            case UnauthorizedAccessException:
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                problemDetails.Status = (int)HttpStatusCode.Unauthorized;
                problemDetails.Title = "No autorizado";
                problemDetails.Detail = exception.Message;
                problemDetails.Type = "https://tools.ietf.org/html/rfc7235#section-3.1";
                break;

            case InvalidOperationException or ArgumentException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                problemDetails.Status = (int)HttpStatusCode.BadRequest;
                problemDetails.Title = "Petición no válida";
                problemDetails.Detail = exception.Message;
                problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                break;

            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                problemDetails.Status = (int)HttpStatusCode.InternalServerError;
                problemDetails.Title = "Error interno del servidor";
                problemDetails.Detail = _env.IsDevelopment()
                    ? exception.Message
                    : "Ocurrió un error inesperado. Por favor, contacte al administrador del sistema.";
                problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";

                if (_env.IsDevelopment())
                {
                    problemDetails.Extensions["stackTrace"] = exception.StackTrace;
                }
                break;
        }

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var json = JsonSerializer.Serialize(problemDetails, jsonOptions);
        return context.Response.WriteAsync(json);
    }
}