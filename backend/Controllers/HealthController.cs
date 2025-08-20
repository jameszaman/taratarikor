using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;

namespace backend.Controllers;

[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    // A route for AWS to check for health
    [HttpGet]
    public IActionResult Get() => Ok(new { status = "Healthy"});

    // For checking if Database is connected properly.
    [HttpGet("db")]
    public async Task<IActionResult> Db([FromServices] AppDbContext db)
    {
        var canConnect = await db.Database.CanConnectAsync();
        return canConnect ?
            Ok(new { status = "Healthy" }) :
            StatusCode(500, new { status = "Unhealthy" });
    }
}

