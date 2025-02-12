using EVS.App.Domain.Abstractions;
using EVS.App.Domain.Abstractions.Repositories;
using EVS.App.Domain.Events;
using EVS.App.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace EVS.App.Infrastructure.Database.Repositories;

public sealed class EventRepository(
    ApplicationDbContext context) : GenericRepository<Event>(context), IEventRepository
{
    private readonly DbSet<Event> _events = context.Events;
    private readonly ApplicationDbContext _context = context;
    
    public async Task<Event?> GetIncludingDependencies(Guid eventId, 
        CancellationToken cancellationToken = default)
        => await ById(eventId).IncludeVoter().IncludeVoterEvents().FirstOrDefaultAsync(cancellationToken);
    
    public async Task<Event?> GetByNameAsync(string eventName, 
        CancellationToken cancellationToken = default)
        => await _events.AsNoTracking().FirstOrDefaultAsync(x => x.Name == eventName, cancellationToken);
    
    public async Task<Event?> GetByIdAsync(Guid eventId, bool includeVoters = false, bool includeCreator = false,
        CancellationToken cancellationToken = default)
        => await ById(eventId).AsNoTracking().FirstOrDefaultAsync(cancellationToken);
    
    public Task<IEnumerable<Event>> GetManyAsPageAsync(int offset = 0, int take = 10, 
        CancellationToken cancellationToken = default)
    {
       var result = _events.AsNoTracking().Skip(offset).Take(take).AsEnumerable();
       return Task.FromResult<IEnumerable<Event>>(result);
    }
    
    public Task<IEnumerable<Event>> GetFullEventsPageAsync(int offset = 0, int take = 10, 
        CancellationToken cancellationToken = default)
    {
        var result = 
            _events
                .IncludeVoter().IncludeVoterEvents()
                .AsNoTracking().Skip(offset).Take(take).AsEnumerable();
        
        return Task.FromResult<IEnumerable<Event>>(result);
    }
}

public static class EventRepositoryExtensions
{
    public static IQueryable<Event> IncludeVoter(this IQueryable<Event> query) 
        => query.Include(x => x.Voter);
    
    public static IQueryable<Event> IncludeVoterEvents(this IQueryable<Event> query)
        => query.Include(x => x.VoterEvents).ThenInclude(x => x.Voter);
}
