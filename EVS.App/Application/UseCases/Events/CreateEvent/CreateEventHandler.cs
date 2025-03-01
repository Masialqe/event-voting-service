using EVS.App.Application.Abstractions;
using EVS.App.Application.Errors;
using EVS.App.Domain.Abstractions;
using EVS.App.Domain.Events;

namespace EVS.App.Application.UseCases.Events.CreateEvent;

public sealed class CreateEventHandler(
    EventService eventService,
    IVoterAccessor voterAccessor,
    ILogger<CreateEventHandler> logger) : IHandler<Result, CreateEventRequest>
{
    public async Task<Result> Handle(CreateEventRequest request, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var currentLoggedVoter = await voterAccessor.GetCurrentLoggedVoterAsync(cancellationToken);
            var voterId = currentLoggedVoter.Value.Id;
            
            var createEventDto = CreateEventDto.Create(request.EventName, request.EventDescription, 
                voterId, request.EventTypes, request.PlayerLimit, request.PointsLimit);
            
            var result = await eventService.CreateEventAsync(createEventDto, cancellationToken);
            
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError("Failed to process event {EventName} due to {ExceptionMessage} {Exception}",
                request.EventName, ex.Message, ex);
            return ApplicationErrors.ApplicationExceptionError;
        }
    }
}