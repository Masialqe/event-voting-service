using EVS.App.Domain.Abstractions;
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
    ApplicationDbContext context) : GenericRepository<VoterEvent>(context), IVoterEventRepository
{
    private readonly DbSet<Event> _events = context.Events;
    private readonly DbSet<Voter> _voters = context.Voters;
    private readonly ApplicationDbContext _context = context;

    public async Task LinkEventAndVoterAsync(VoterEvent voterEvent, 
        CancellationToken cancellationToken = default)
    {
        var voterState = await _voters.GetByIdIncludingEvents(voterEvent.VoterId, cancellationToken)
                ?? throw new InvalidOperationException("Voter event was not found");
        
        var eventState = await _events.GetByIdIncludingEvents(voterEvent.EventId, cancellationToken)
                ?? throw new EventNotFoundException("Event was not found");
        
        voterState.AddVoterEvent(voterEvent);
        eventState.AddVoterEvent(voterEvent);

        _context.VoterEvent.Add(voterEvent);
        
        await _context.SaveChangesAsync(cancellationToken);
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

