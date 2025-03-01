using EVS.App.Domain.Abstractions.Repositories;
using EVS.App.Domain.Abstractions;
using EVS.App.Domain.Exceptions;
using EVS.App.Domain.Events;

namespace EVS.App.Domain.VoterEvents;

public sealed class VoterEventService(
    IVoterEventRepository voterEventRepository,
    ITransactionService transactionService,
    IEventRepository eventRepository)
{
    //todo: add also event update to transaction and refactor
    public async Task<Result<EventVotingState>> RegisterVoteAsync(SaveVoterScoreRequest[] saveVoterScoreRequests,
        Guid eventId, Guid voteMadeById,
        CancellationToken cancellationToken = default)
    {
        if (saveVoterScoreRequests.Length == 0) return DomainErrors.NoElementToProcessError;
        if (saveVoterScoreRequests.Any(x => x.VoterId == voteMadeById)) return VoterEventErrors.CannotVoteForSelfError;
        
        try
        {
            var voterResult = await SetVoterAsAlreadyVotedAsync(voteMadeById, eventId, cancellationToken);
            if(voterResult.IsFailure) return voterResult.Error;
            
            var eventResult = await AddVoteToRelatedEventAsync(eventId, cancellationToken);
            if(eventResult.IsFailure) return eventResult.Error;
            
            await transactionService.RegisterVoteAsync(saveVoterScoreRequests, eventResult.Value,
                voterResult.Value, cancellationToken);    
            
            return eventResult.Value.HasAllVotersVoted
                ? EventVotingState.AllVoted
                : EventVotingState.NotAllVoted;
        }
        catch (Exception)
        {
            return VoterEventErrors.VoterEventStateCannotBeProcessedError;
        }
    }
    
    private async Task<Result<VoterEvent>> SetVoterAsAlreadyVotedAsync(Guid votingVoterId, Guid eventId,
        CancellationToken cancellationToken = default)
    {
        var voterEventState = await voterEventRepository.GetByRelatedVoterAndEvent(votingVoterId, eventId, cancellationToken);
        if (voterEventState is null) return VoterEventErrors.VoterEventNotFoundError;
        
        voterEventState.SetHasVoted();
        return voterEventState;
    }
    
    private async Task<Result<Event>> AddVoteToRelatedEventAsync(Guid relatedEventId,
        CancellationToken cancellationToken)
    {
        var relatedEventState = await eventRepository.GetByIdAsync(relatedEventId, cancellationToken);
        if (relatedEventState == null) return EventErrors.EventNotFoundError;
            
        relatedEventState.AddVote();
        return relatedEventState;
    }
}

