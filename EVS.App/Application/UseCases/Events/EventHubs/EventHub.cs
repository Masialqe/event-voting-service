using EVS.App.Application.Abstractions;
using EVS.App.Application.Errors;
using EVS.App.Domain.Events;
using EVS.App.Domain.Voters;
using Microsoft.AspNetCore.SignalR;

namespace EVS.App.Application.UseCases.Events.EventHubs;

//todo: fix code duplications in exceptions handling
//todo: add auth to hub
public class EventHub(
    IEventNotifier eventNotifier,
    EventService eventService,
    ILogger<EventHub> logger) : Hub<IEventHub>
{
    //todo: refactor 
    public async Task JoinGroupAsync(string eventIdAsString)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, eventIdAsString);
    }

    public async Task RemoveFromGroupAsync(string eventIdAsString)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, eventIdAsString);
    }

    public async Task StartEventAsync(string eventIdAsString)
    {
        try
        {
            var eventIdAsGuid = Guid.Parse(eventIdAsString);
            var result = await eventService.StartEventAsync(eventIdAsGuid);
            
            if (!result.IsSuccess)
            {
                await eventNotifier.ReturnErrorReponseToClient(Context.ConnectionId, result.Error);
            }
            else
            {
                await eventNotifier.BroadcastEventStarted(eventIdAsString);
            }
        }
        catch (Exception ex)
        {
            await ReturnErrorResponseAsync(Context, ex);
        }
    }

    public async Task EndEventAsync(string eventIdAsString)
    {
        try
        {
            var eventIdAsGuid = Guid.Parse(eventIdAsString);
            var result = await eventService.EndEventAsync(eventIdAsGuid);
            
            if (!result.IsSuccess)
            {
                await eventNotifier.ReturnErrorReponseToClient(Context.ConnectionId, result.Error);
            }
            else
            {
                await eventNotifier.BroadcastEventEnded(eventIdAsString);
            }
        }
        catch (Exception ex)
        {
            await ReturnErrorResponseAsync(Context, ex);
        }
    }
    
    public async Task AddVisitorToEventAsync(string voterIdAsString, 
        string eventIdAsString)
    {
        try
        {
            var voterIdAsGuid = Guid.Parse(voterIdAsString);
            var eventIdAsGuid = Guid.Parse(eventIdAsString);
            
            var result = await eventService.AddVoterToEventAsync(eventIdAsGuid, voterIdAsGuid);
            
            if (!result.IsSuccess)
            {
                await eventNotifier.ReturnErrorReponseToClient(Context.ConnectionId, result.Error);
            }
            else
            {
                await eventNotifier.VisitorAddedSuccessfullyResponse(Context.ConnectionId, eventIdAsGuid.ToString());
                await eventNotifier.BroadcastEventsVisitorAdded(eventIdAsString, result.Value);
            }
        }
        catch (Exception ex)
        {
            await ReturnErrorResponseAsync(Context, ex);
        }
    }

    private async Task ReturnErrorResponseAsync(HubCallerContext context, Exception exception)
    {
        logger.LogError("Failed to process hub operation due to {ExceptionMessage} {Exception}",
            exception.Message, exception);
        await eventNotifier.ReturnErrorReponseToClient(Context.ConnectionId, ApplicationErrors.ApplicationExceptionError);
    }
}

