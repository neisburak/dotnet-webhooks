using Api.Models.Dtos;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController, Route("api/[controller]")]
public class FlightsController : ControllerBase
{
    private readonly IFlightService _flightService;

    public FlightsController(IFlightService flightService)
    {
        _flightService = flightService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        var flights = await _flightService.GetAsync(cancellationToken);

        return Ok(flights);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id, CancellationToken cancellationToken)
    {
        var flight = await _flightService.GetAsync(id, cancellationToken);

        return flight is null ? NotFound() : Ok(flight);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] FlightForUpsert flightForInsert, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _flightService.AddAsync(flightForInsert, cancellationToken);

            return CreatedAtRoute("GetAsync", new { Code = result.Code }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PostAsync(int id, [FromBody] FlightForUpsert flightForUpdate, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _flightService.UpdateAsync(id, flightForUpdate, cancellationToken);

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}