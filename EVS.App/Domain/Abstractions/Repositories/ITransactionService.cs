using EVS.App.Domain.Events;
using EVS.App.Domain.VoterEvents;

namespace EVS.App.Domain.Abstractions.Repositories;

public interface ITransactionService
{
    Task RegisterVoteAsync(SaveVoterScoreRequest[] request, Event relatedEventState, 
        VoterEvent voteOwnerState, CancellationToken cancellationToken = default);
}