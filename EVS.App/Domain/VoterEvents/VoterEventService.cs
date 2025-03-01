using EVS.App.Domain.Abstractions.Repositories;
using EVS.App.Domain.Abstractions;
using EVS.App.Domain.Events;
using EVS.App.Domain.Exceptions;
using EVS.App.Domain.Voters;

namespace EVS.App.Domain.VoterEvents;

public sealed class VoterEventService(
    IVoterEventRepository voterEventRepository,
    IVoterRepository voterRepository,
    IEventRepository eventRepository)
{
    //todo: add also event update to transaction
    public async Task<Result<EventVotingState>> SaveScoreForManyVotersAsync(SaveVoterScoreRequest[] saveVoterScoreRequests,
        Guid eventId, Guid voteMadeById,
        CancellationToken cancellationToken = default)
    {
        if (saveVoterScoreRequests.Length == 0) return DomainErrors.NoElementToProcessError;
        
        if (!await eventRepository.IsAnyExists(eventId, cancellationToken)) return EventErrors.EventNotFoundError;
        if(!await voterRepository.IsAnyExists(voteMadeById, cancellationToken)) return VoterErrors.VoterNotFoundError;
        
        try
        {
            var result = await SetVoterAsAlreadyVotedAsync(voteMadeById, eventId, cancellationToken);
            if(result.IsFailure) return result.Error;
        
            var hasAllVotersMadeVote = await AddVoteToRelatedEventAsync(eventId, cancellationToken);
            if(result.IsFailure) return result.Error;
            
            await voterEventRepository.UpdateVoterEventScoresAsync(saveVoterScoreRequests, cancellationToken);
            
            return hasAllVotersMadeVote;
        }
        catch (Exception ex)
        {
            return VoterEventErrors.VoterEventStateCannotBeProcessedError;
        }
    }
    
    private async Task<Result> SetVoterAsAlreadyVotedAsync(Guid votingVoterId, Guid eventId,
        CancellationToken cancellationToken = default)
    {
        var voterEventState = await voterEventRepository.GetByRelatedVoterAndEvent(votingVoterId, eventId, cancellationToken);
        if (voterEventState is null) return VoterEventErrors.VoterEventNotFoundError;
        
        voterEventState.SetHasVoted();
        await voterEventRepository.UpdateAsync(voterEventState, cancellationToken);
        
        return Result.Success();
    }
    
    private async Task<Result<EventVotingState>> AddVoteToRelatedEventAsync(Guid relatedEventId,
        CancellationToken cancellationToken)
    {
        var relatedEventState = await eventRepository.GetIncludingDependencies(relatedEventId, cancellationToken);
        if (relatedEventState == null) return EventErrors.EventNotFoundError;
            
        relatedEventState.AddVote();
        await eventRepository.UpdateAsync(relatedEventState, cancellationToken);
        
        return relatedEventState.HasAllVotersVoted
            ? EventVotingState.AllVoted
            : EventVotingState.NotAllVoted;
    }
}

