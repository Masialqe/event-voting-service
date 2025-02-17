using EVS.App.Domain.Abstractions.Entities;
using EVS.App.Domain.Abstractions.Repositories;
using EVS.App.Domain.Events;
using EVS.App.Domain.Exceptions;
using EVS.App.Domain.VoterEvents;
using EVS.App.Domain.Voters;
using EVS.App.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace EVS.App.Infrastructure.Database.Repositories;

public sealed class VoterEventRepository(
    IDbContextFactory<ApplicationDbContext> contextFactory) : GenericRepository<VoterEvent>(contextFactory), IVoterEventRepository
{
    public async Task LinkEventAndVoterAsync(VoterEvent voterEvent, 
        CancellationToken cancellationToken = default)
    {
        await SaveAsync(async context =>
        {
            var voterState = await context.Set<Voter>().GetByIdIncludingEvents(voterEvent.VoterId, cancellationToken)
                             ?? throw new InvalidOperationException($"Voter was not found {voterEvent.VoterId}");
        
            var eventState = await context.Set<Event>().GetByIdIncludingEvents(voterEvent.EventId, cancellationToken)
                             ?? throw new EventNotFoundException("Event was not found");
        
            eventState.AddVoterEvent(voterEvent);
            voterState.AddVoterEvent(voterEvent);

            context.VoterEvent.Add(voterEvent);
        }, cancellationToken);
    }
}

internal static class VoterEventRepositoryExtensions
{
    public static async Task<T?> GetByIdIncludingEvents<T>(this DbSet<T> dbSet, Guid id,
        CancellationToken cancellationToken = default) where T : class, IVoterEventEntity
    {
        return await dbSet.AsTracking()
            .Include(x => x.VoterEvents)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}


