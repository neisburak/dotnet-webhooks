using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAgent.Data;
using TravelAgent.Models;

namespace TravelAgent.Controllers;

[ApiController, Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly TravelAgentDbContext _context;
    private readonly ILogger<NotificationsController> _logger;

    public NotificationsController(ILogger<NotificationsController> logger, TravelAgentDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] ChangePayload payload)
    {
        _logger.LogInformation($"Webhook received from: {payload.Publisher}");

        var model = await _context.SubscriptionSecrets.FirstOrDefaultAsync(f => f.Publisher == payload.Publisher && f.Secret == payload.Secret);
        if (model is null)
        {
            _logger.LogWarning("Invalid secret - Ignore webhook.");
            return BadRequest();
        }

        _logger.LogInformation($"Valid webhook! New price: {payload.NewPrice}");
        return Ok();
    }
}