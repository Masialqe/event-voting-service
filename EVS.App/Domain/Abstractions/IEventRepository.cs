using EVS.App.Domain.Events;

namespace EVS.App.Domain.Abstractions;

public interface IEventRepository
{
    Task CreateAsync(Event newEvent, 
        CancellationToken cancellationToken = default);
    Task<Event?> GetByNameAsync(string eventName, 
        CancellationToken cancellationToken = default);
    Task<Event?> GetByIdAsync(Guid eventId, bool includeVoters = false,
        CancellationToken cancellationToken = default);
    Task UpdateAsync(Event updatedState, 
        CancellationToken cancellationToken = default);
    Task<IEnumerable<Event>> GetEventsPageAsync(int offset = 0, int take = 10, 
        CancellationToken cancellationToken = default);
}