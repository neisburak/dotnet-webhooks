using Api.Models.Dtos;

namespace Api.Services;

public interface IFlightService
{
    Task<IEnumerable<FlightForView>> GetAsync(CancellationToken cancellationToken = default);
    Task<FlightForView?> GetAsync(int id, CancellationToken cancellationToken = default);
    Task<FlightForView> AddAsync(FlightForUpsert flightForInsert, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, FlightForUpsert flightForUpdate, CancellationToken cancellationToken = default);
}