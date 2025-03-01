using EVS.App.Domain.Abstractions.Repositories;
using EVS.App.Domain.Events;
using EVS.App.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace EVS.App.Infrastructure.Database.Repositories;

public sealed class EventRepository(
    IDbContextFactory<ApplicationDbContext> contextFactory) 
    : GenericRepository<Event>(contextFactory), IEventRepository
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory = contextFactory;

    //todo: consider if not add start, end separate tasks
    public async Task<Event?> GetIncludingDependencies(Guid eventId, 
        CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(
            context => context.Set<Event>()
                .Where(e => e.Id == eventId)
                .IncludeVoter()
                .IncludeVoterEvents()
                .FirstOrDefaultAsync(cancellationToken),
            cancellationToken);
    }
    
    public async Task<Event?> GetByNameAsync(string eventName, 
        CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(
            context => context.Set<Event>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name == eventName, cancellationToken),
            cancellationToken);
    }
    
    public async Task<IEnumerable<Event>> GetManyAsPageAsync(int offset = 0, int take = 50, 
        CancellationToken cancellationToken = default)
    { 
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
    
        var result = await context.Set<Event>()
            .AsNoTracking()
            .IncludeVoterEvents()
            .Skip(offset)
            .Take(take)
            .ToListAsync(cancellationToken);
    
        return result;
        //todo: change it to this
        // return await ExecuteAsync(
        //     context => await context.Set<Event>()
        //         .AsNoTracking()
        //         .IncludeVoterEvents()
        //         .Skip(offset)
        //         .Take(take)
        //         .ToListAsync(cancellationToken),
        //     cancellationToken);
    }
    public async Task DeleteEndedOlderThanDay(CancellationToken cancellationToken = default)
    {
        const int MAX_HOURS_TO_DELETE = -12;
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        await context.Set<Event>()
            .Where(x => x.EndedAt < DateTime.UtcNow.AddHours(MAX_HOURS_TO_DELETE))
                .ExecuteDeleteAsync(cancellationToken);
    }
}


public static class EventRepositoryExtensions
{
    public static IQueryable<Event> IncludeVoter(this IQueryable<Event> query) 
        => query.Include(x => x.Voter);
    
    public static IQueryable<Event> IncludeVoterEvents(this IQueryable<Event> query)
        => query.Include(x => x.VoterEvents).ThenInclude(x => x.Voter);
}
