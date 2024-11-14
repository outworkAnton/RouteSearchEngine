using Microsoft.AspNetCore.Mvc;

namespace RouteSearchEngine.Controllers;

[ApiController]
[Route("ping")]
public class HealthCheckController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> HealthCheck() => Ok();
}