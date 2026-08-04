using Microsoft.AspNetCore.Mvc;

namespace TraceX.Api.Controllers;

// Estos atributos son obligatorios para que .NET sepa que esta clase atiende peticiones HTTP
[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    // Esto indica que responderemos a peticiones de tipo GET
    [HttpGet]
    public IActionResult CheckStatus()
    {
        return Ok(new
        {
            Message = "TraceX API Operativa",
            Environment = "Development",
            Timestamp = DateTime.UtcNow
        });
    }
    
    // [HttpPost]
}

