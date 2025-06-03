using EVS.App.Domain.Events;

namespace EVS.App.Domain.Abstractions.Repositories;

public interface IEventRepository : IGenericRepository<Event>
{
    Task<Event?> GetIncludingDependencies(Guid eventId,
        CancellationToken cancellationToken = default);

    Task<Event?> GetIncludingVoterEvents(Guid eventId,
        CancellationToken cancellationToken = default);
    
    Task<bool> IsEventNameAlreadyExistsAsync(string eventName, 
        CancellationToken cancellationToken = default);
    
    Task<IEnumerable<Event>> GetManyAsync(int offset = 0, int take = 50,
        CancellationToken cancellationToken = default);

    Task<(IEnumerable<Event> Events, int TotalCount)> GetPageByNameAsync(string? eventName, int offset = 0, int take = 50,
        CancellationToken cancellationToken = default);
    Task DeleteEndedOlderThanDay(CancellationToken cancellationToken = default);
}