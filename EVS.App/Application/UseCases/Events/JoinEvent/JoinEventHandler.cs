using EVS.App.Application.Abstractions;
using EVS.App.Application.Errors;
using EVS.App.Domain.Abstractions;
using EVS.App.Domain.Events;

namespace EVS.App.Application.UseCases.Events.JoinEvent;

public class JoinEventHandler(
    EventService eventService,
    ILogger<JoinEventHandler> logger) : IHandler<Result, JoinEventRequest>
{
    public async Task<Result> Handle(JoinEventRequest request, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var eventId = request.EventId;
            var voterId = request.Voter.Id;

            var result = await eventService.AddVoterToEventAsync(eventId, voterId, cancellationToken);
            return !result.IsSuccess ? result : Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError("Failed add visitor due to {ExceptionMessage} {Exception}",
                ex.Message, ex);
            return ApplicationErrors.ApplicationExceptionError;
        }
    }
}