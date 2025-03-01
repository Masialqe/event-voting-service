using EVS.App.Domain.Abstractions.Entities;
using EVS.App.Domain.Abstractions.Repositories;
using EVS.App.Domain.Events;
using EVS.App.Domain.Exceptions;
using EVS.App.Domain.VoterEvents;
using EVS.App.Domain.Voters;
using EVS.App.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace EVS.App.Infrastructure.Database.Repositories;

/// <summary>
/// <see cref="VoterEvent"/> related repository.
/// </summary>
public sealed class VoterEventRepository(
    IDbContextFactory<ApplicationDbContext> contextFactory)
    : GenericRepository<VoterEvent>(contextFactory), IVoterEventRepository
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory = contextFactory;

    /// <summary>
    /// Links an event with a voter using the <see cref="VoterEvent"/> entity.
    /// </summary>
    /// <param name="voterEvent">The <see cref="VoterEvent"/> object representing the link between the voter and the event.</param>
    /// <param name="cancellationToken">An optional cancellation token.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the voter included in the <paramref name="voterEvent"/> object does not exist.
    /// </exception>
    /// <exception cref="EventNotFoundException">
    /// Thrown when the event included in the <paramref name="voterEvent"/> object does not exist.
    /// </exception>

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
    
    /// <summary>
    /// Returns given <see cref="VoterEvent"/> score.
    /// </summary>
    /// <param name="voterEventId">Id as Guid.</param>
    /// <param name="cancellationToken">An optional cancellation token.</param>
    /// <returns><see cref="VoterEvent"/> score as int. </returns>
    public async Task<int> GetVoterScoreAsync(Guid voterEventId,
        CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(
            context => context.Set<VoterEvent>()
                .AsNoTracking()
                .Where(x => x.Id == voterEventId)
                .Select(x => x.Score)
                .FirstOrDefaultAsync(cancellationToken),
            cancellationToken);
    }

    public async Task UpdateVoterEventScoresAsync(SaveVoterScoreRequest[] saveVoterScoreRequests,
        CancellationToken cancellationToken = default)
    {
        await SaveTransactionAsync(async context =>
        {
            var voterEventsId = saveVoterScoreRequests.Select(x => x.VoterId).ToArray();
            var voterEventStates = await context.Set<VoterEvent>()
                .Where(x => voterEventsId.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id, cancellationToken);
        
            foreach (var saveVoterScoreRequest in saveVoterScoreRequests)
            {
                if (!voterEventStates.TryGetValue(saveVoterScoreRequest.VoterId, out var currentState))
                {
                    throw new ArgumentException($"Voter event not found {saveVoterScoreRequest.VoterId}");
                }

                currentState.AddScore(saveVoterScoreRequest.Score);
            }
        }, cancellationToken);
    }

    public async Task<VoterEvent?> GetIncludingEvent(Guid voterEventId, 
        CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(
            context => context.Set<VoterEvent>()
                .Include(e => e.Event)
                .FirstOrDefaultAsync(x => x.Id == voterEventId, cancellationToken),
            cancellationToken);
    }

    public async Task<VoterEvent?> GetByRelatedVoterAndEvent(Guid relatedVoterId, Guid relatedEventId, 
        CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(
            context => context.Set<VoterEvent>()
                .Where(x => x.VoterId == relatedVoterId && x.EventId == relatedEventId)
                .FirstOrDefaultAsync(cancellationToken),
            cancellationToken);
    }
    
    public override async Task<VoterEvent?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(
            context => context.Set<VoterEvent>()
                .AsNoTracking()
                .Include(e => e.Voter)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken),
            cancellationToken);
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


