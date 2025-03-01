using EVS.App.Application.Abstractions;
using EVS.App.Application.Errors;
using EVS.App.Domain.Abstractions;
using EVS.App.Domain.Events;

namespace EVS.App.Application.UseCases.Events.GetEventById;

public class GetEventByIdHandler(
    EventService eventService,
    ILogger<GetEventByIdHandler> logger) : IHandler<Result<Event>, GetEventByIdRequest>
{
    public async Task<Result<Event>> Handle(GetEventByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var eventIdAsGuid = Guid.Parse(request.EventId);
            var result = await eventService.GetEventByIdAsync(eventIdAsGuid, cancellationToken);
            return result;  
        }
        catch (Exception ex)
        {
            logger.LogError("Failed fetching event due to {ExceptionMessage} {Exception}",
                ex.Message, ex);
            return ApplicationErrors.ApplicationExceptionError;
        }
    }
}