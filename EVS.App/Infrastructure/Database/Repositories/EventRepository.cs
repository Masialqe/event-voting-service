using EVS.App.Domain.Abstractions;
using EVS.App.Domain.Events;
using EVS.App.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace EVS.App.Infrastructure.Database.Repositories;

public sealed class EventRepository(
    ApplicationDbContext context) : IEventRepository
{
    private readonly DbSet<Event> _events = context.Events;
    public async Task CreateAsync(Event newEvent, 
        CancellationToken cancellationToken = default)
    {
        await _events.AddAsync(newEvent, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Event?> GetByNameAsync(string eventName, 
        CancellationToken cancellationToken = default)
    {
        var result = await _events
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == eventName, cancellationToken);

        return result;
    }
    
    public async Task<Event?> GetByIdAsync(Guid eventId, bool includeVoters = false,
        CancellationToken cancellationToken = default)
    {
        var result = _events
            .Where(x => x.Id == eventId);
        
        if(includeVoters) result = result.Include(x => x.Voter);
        
        return await result.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateAsync(Event updatedState, 
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(updatedState);
        
        var entityState = _events.Entry(updatedState);
        
        if(entityState.State == EntityState.Detached) _events.Attach(updatedState);
        
        entityState.State = EntityState.Modified;
        
        await context.SaveChangesAsync(cancellationToken);
    }

    public Task<IEnumerable<Event>> GetEventsPageAsync(int offset = 0, int take = 10, 
        CancellationToken cancellationToken = default)
    {
       var result = _events
           .AsNoTracking()
           .Skip(offset)
           .Take(take);

       return Task.FromResult<IEnumerable<Event>>(result);
    }
}