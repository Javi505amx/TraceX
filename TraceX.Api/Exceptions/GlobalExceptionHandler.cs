using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TraceX.Api.Exceptions;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    // 1. Inyectamos el logger nativo de .NET

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        // 2. Registramos el error real en los logs del servidor
        logger.LogError(exception, "Ocurrió una excepción no controlada: {Message}", exception.Message);

        // 3. Configuramos la respuesta HTTP segura
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = "application/json";

        // 4. Creamos el objeto ProblemDetails estándar
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Internal Server Error",
            Detail = "Ocurrió un error inesperado en el servidor. Por favor, contacte al administrador."
        };

        // 5. Escribimos el JSON directamente en la respuesta HTTP
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        // 6. Le decimos a .NET que la excepción ya fue manejada con éxito
        return true;
    }
}