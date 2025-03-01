using EVS.App.Domain.VoterEvents;

namespace EVS.App.Domain.Abstractions.Repositories;

public interface IVoterEventRepository : IGenericRepository<VoterEvent>
{
    Task LinkEventAndVoterAsync(VoterEvent voterEvent, 
        CancellationToken cancellationToken = default);
    Task UpdateVoterEventScoresAsync(SaveVoterScoreRequest[] saveVoterScoreRequests,
        CancellationToken cancellationToken = default);
    Task<int> GetVoterScoreAsync(Guid voterEventId,
        CancellationToken cancellationToken = default);
    Task<VoterEvent?> GetIncludingEvent(Guid voterEventId,
        CancellationToken cancellationToken = default);

    Task<VoterEvent?> GetByRelatedVoterAndEvent(Guid relatedVoterId, Guid relatedEventId,
        CancellationToken cancellationToken = default);
}