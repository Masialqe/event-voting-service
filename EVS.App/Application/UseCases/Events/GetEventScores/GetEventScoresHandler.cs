using EVS.App.Application.Abstractions;
using EVS.App.Application.Errors;
using EVS.App.Domain.Abstractions;
using EVS.App.Domain.VoterEvents;
using EVS.App.Domain.Events;

namespace EVS.App.Application.UseCases.Events.GetEventScores;

//todo: idea - create Handler base class with logger and wrapper to try-catch Handle method
public class GetEventScoresHandler(
    EventService eventService,
    ILogger<GetEventScoresHandler> logger) : IHandler<Result<VoterScoreDto[]>, GetEventScoresRequest>
{
    public async Task<Result<VoterScoreDto[]>> Handle(GetEventScoresRequest request, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await eventService.GetEventByIdAsync(request.EventId, cancellationToken);
            
            return result.IsSuccess 
                ? result.Value?.GetVotersScores() 
                : result.Error;
        }
        catch (Exception ex)
        {
            logger.LogError("Failed to process voter scores list due to {ExceptionMessage} {Exception}",
                ex.Message, ex);
            return ApplicationErrors.ApplicationExceptionError;
        }
    }
}