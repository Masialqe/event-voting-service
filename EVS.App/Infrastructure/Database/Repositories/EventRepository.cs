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
    
    public async Task<Event?> GetIncludingDependencies(Guid eventId, 
        CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(
            context => context.Set<Event>()
                .AsNoTracking()
                .Where(e => e.Id == eventId)
                .IncludeVoter()
                .IncludeVoterEvents()
                .FirstOrDefaultAsync(cancellationToken),
            cancellationToken);
    }
    
    public async Task<Event?> GetIncludingVoterEvents(Guid eventId, 
        CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(
            context => context.Set<Event>()
                .AsNoTracking()
                .Where(e => e.Id == eventId)
                .IncludeVoterEvents()
                .FirstOrDefaultAsync(cancellationToken),
            cancellationToken);
    }
    
    
    public async Task<bool> IsEventNameAlreadyExistsAsync(string eventName,CancellationToken cancellationToken = default)
     => await ExecuteAsync(context 
         => context.Set<Event>().AnyAsync(x => x.Name == eventName, cancellationToken), cancellationToken);
    
    public async Task<IEnumerable<Event>> GetManyAsync(int offset = 0, int take = 50, 
        CancellationToken cancellationToken = default)
            => await ExecuteAsync(context => context.Set<Event>().AsNoTracking()
                .OrderByDescending(e => e.CreatedAt)
                .IncludeVoterEvents()
                .Skip(offset)
                .Take(take)
                .ToListAsync(cancellationToken), cancellationToken);
    public async Task<(IEnumerable<Event> Events, int TotalCount)> GetPageByNameAsync(
        string? eventName, int offset = 0, int take = 50, CancellationToken cancellationToken = default)
            => await ExecuteAsync(async context =>
            {
                var query = context.Set<Event>().AsNoTracking();

                if (!string.IsNullOrEmpty(eventName)) query = query.Where(e 
                    => e.Name.Contains(eventName));
                
                var totalRecordCount = await query.CountAsync(cancellationToken);

                var eventList = await query.OrderByDescending(e => e.CreatedAt)
                    .IncludeVoterEvents()
                    .Skip(offset)
                    .Take(take)
                    .ToListAsync(cancellationToken);
                
                return (eventList, totalRecordCount);
            }, cancellationToken);
    
    public async Task DeleteEndedOlderThanDay(CancellationToken cancellationToken = default)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        await context.Set<Event>()
            .Where(x => x.EndedAt < DateTime.UtcNow.AddDays(-1))
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
