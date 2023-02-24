using Api.Models.Dtos;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController, Route("api/[controller]")]
public class WebhookSubscriptionsController : ControllerBase
{
    private readonly IWebhookSubscriptionService _subscriptionService;

    public WebhookSubscriptionsController(IWebhookSubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        var flights = await _subscriptionService.GetAsync(cancellationToken);

        return Ok(flights);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id, CancellationToken cancellationToken)
    {
        var subscription = await _subscriptionService.GetAsync(id, cancellationToken);

        return subscription is null ? NotFound() : Ok(subscription);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] WebhookSubscriptionForUpsert subscriptionForInsert, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _subscriptionService.AddAsync(subscriptionForInsert, cancellationToken);

            return CreatedAtRoute("GetAsync", new { Secret = result.Secret }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PostAsync(int id, [FromBody] WebhookSubscriptionForUpsert subscriptionForUpdate, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _subscriptionService.UpdateAsync(id, subscriptionForUpdate, cancellationToken);

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}