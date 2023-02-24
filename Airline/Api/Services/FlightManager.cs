using Api.Data;
using Api.MessageBus;
using Api.Models;
using Api.Models.Dtos;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Api.Services;

public class FlightManager : IFlightService
{
    private readonly ILogger<FlightManager> _logger;
    private readonly IMessageBus _messageBus;
    private readonly AirlineDbContext _context;

    public FlightManager(IMessageBus messageBus, AirlineDbContext context, ILogger<FlightManager> logger)
    {
        _logger = logger;
        _context = context;
        _messageBus = messageBus;
    }

    public async Task<IEnumerable<FlightForView>> GetAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await _context.Flights.ProjectToType<FlightForView>().ToListAsync(cancellationToken);
    }

    public async Task<FlightForView?> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var flight = await _context.Flights.FirstOrDefaultAsync(f => f.Id == id, cancellationToken);

        return flight?.Adapt<FlightForView>();
    }

    public async Task<FlightForView?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var flight = await _context.Flights.FirstOrDefaultAsync(f => f.Code == code, cancellationToken);

        return flight?.Adapt<FlightForView>();
    }

    public async Task<FlightForView> AddAsync(FlightForUpsert flightForInsert, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (await _context.Flights.AnyAsync(a => a.Code == flightForInsert.Code, cancellationToken))
        {
            throw new Exception($"Flight with code '{flightForInsert.Code}' already exist.");
        }

        var flight = flightForInsert.Adapt<Flight>();

        await _context.AddAsync(flight, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return flight.Adapt<FlightForView>();
    }

    public async Task<bool> UpdateAsync(int id, FlightForUpsert flightForUpdate, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var flight = await _context.Flights.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        if (flight is null) throw new Exception($"Flight with id '{id}' doesn't found.");

        var oldPrice = flight.Price;
        
        flight.Code = flightForUpdate.Code;
        flight.Price = flightForUpdate.Price;

        _context.Update(flight);
        var result = await _context.SaveChangesAsync() > 0;

        if (oldPrice != flight.Price)
        {
            _logger.LogInformation($"Flight price changed from {oldPrice} to {flight.Price}.");

            _messageBus.Publish(new()
            {
                WebhookType = WebhookType.PriceChange,
                FlightCode = flight.Code,
                NewPrice = flight.Price,
            });
        }

        return result;
    }
}