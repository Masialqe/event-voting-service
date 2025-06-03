using EVS.App.Application.Abstractions;
using EVS.App.Application.Errors;
using EVS.App.Domain.Abstractions;
using EVS.App.Domain.Abstractions.Repositories;
using EVS.App.Domain.Events;

namespace EVS.App.Application.UseCases.Events.ListEvents;

public sealed class LoadEventPageByNameHandler(
    IEventRepository eventRepository,
    ILogger<LoadEventPageByNameHandler> logger) : IHandler<Result<(Event[], int)>, LoadEventPageByNameRequest>
{
    public async Task<Result<(Event[], int)>> Handle(LoadEventPageByNameRequest request, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var offset = (request.PageNumber - 1) * request.PageSize;
            
            var (eventListResult, totalEventCount) = await eventRepository.GetPageByNameAsync(
                request.EventName, offset, request.PageSize, cancellationToken);
            
            return (eventListResult.ToArray(), totalEventCount);
        }
        catch (Exception ex)
        {
            logger.LogError("Failed to process event list of events due to {ExceptionMessage} {Exception}",
                 ex.Message, ex);
            return ApplicationErrors.ApplicationExceptionError;
        }
    }
}