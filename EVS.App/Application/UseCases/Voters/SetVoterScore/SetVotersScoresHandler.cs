using EVS.App.Application.Abstractions;
using EVS.App.Application.Errors;
using EVS.App.Domain.Abstractions;
using EVS.App.Domain.Events;
using EVS.App.Domain.VoterEvents;

namespace EVS.App.Application.UseCases.Voters.SetVoterScore;

public sealed class SetVotersScoresHandler(
    VoterEventService voterEventService,
    EventService eventService,
    IEventNotifier eventNotifier,
    ILogger<SetVotersScoresHandler> logger) : IHandler<Result, SetVotersScoresRequest>
{
    public async Task<Result> Handle(SetVotersScoresRequest request, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result  = await voterEventService.RegisterVoteAsync(request.VotersScores, 
                    request.EventId, request.VoteMadeById, cancellationToken);

            if (result.IsFailure) return result.Error;
            
            if (result.Value == EventVotingState.AllVoted)
            {
                var eventId = request.EventId;
                await eventService.EndEventAsync(eventId, cancellationToken);
                await eventNotifier.BroadcastEventEnded(eventId.ToString());
            }
            
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError("Failed to set visitors scores due to {ExceptionMessage} {Exception}",
                ex.Message, ex);
            return ApplicationErrors.ApplicationExceptionError;
        }
    }
}