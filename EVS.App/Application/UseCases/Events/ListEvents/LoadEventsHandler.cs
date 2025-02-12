using EVS.App.Application.Abstractions;
using EVS.App.Application.Errors;
using EVS.App.Domain.Abstractions;
using EVS.App.Domain.Abstractions.Repositories;
using EVS.App.Domain.Events;

namespace EVS.App.Application.UseCases.Events.ListEvents;

public sealed class LoadEventsHandler(
    IEventRepository eventRepository,
    ILogger<LoadEventsHandler> logger) : IHandler<Result<Event[]>, LoadEventsRequest>
{
    public async Task<Result<Event[]>> Handle(LoadEventsRequest request, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await eventRepository.GetManyAsPageAsync(cancellationToken: cancellationToken);
            
            return result.ToArray();
        }
        catch (Exception ex)
        {
            logger.LogError("Failed to process event list of events due to {ExceptionMessage} {Exception}",
                 ex.Message, ex);
            return ApplicationErrors.ApplicationExceptionError;
        }
    }
}