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
    public async Task<IActionResult> PostAsync([FromBody] ChangePayload payload, CancellationToken cancellationToken = default)
    {
        var secret = GetHeader("Secret");
        var publisher = GetHeader("Publisher");
        var webhookType = GetHeader("Event-Type");

        if (!await CheckSubscriptionAsync(publisher, Guid.Parse(secret ?? default!), cancellationToken))
        {
            _logger.LogWarning("Invalid secret - Ignore webhook.");
            return BadRequest();
        }

        _logger.LogInformation($"Webhook received from: {publisher}");

        var model = await _context.Flights.FirstOrDefaultAsync(f => f.Code == payload.FlightCode, cancellationToken);
        if (model is null)
        {
            model = new() { Code = payload.FlightCode, Price = payload.NewPrice };
            await _context.AddAsync(model, cancellationToken);
        }
        else
        {
            model.Price = payload.NewPrice;
            _context.Update(model);
        }

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"Valid webhook! New price: {payload.NewPrice}");
        return Ok();
    }

    private async Task<bool> CheckSubscriptionAsync(string? publisher, Guid? secret, CancellationToken cancellationToken = default)
    {
        return await _context.SubscriptionSecrets.AnyAsync(f => f.Publisher == publisher && f.Secret == secret, cancellationToken);
    }
    private string? GetHeader(string key) => Request.Headers.ContainsKey(key) ? Request.Headers[key] : default!;
}