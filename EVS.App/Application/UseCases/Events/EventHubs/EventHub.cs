using EVS.App.Application.Abstractions;
using EVS.App.Application.Errors;
using EVS.App.Domain.Events;
using EVS.App.Domain.Voters;
using Microsoft.AspNetCore.SignalR;

namespace EVS.App.Application.UseCases.Events.EventHubs;

//todo: fix code duplications in exceptions handling
public class EventHub(
    IEventNotifier eventNotifier,
    EventService eventService,
    ILogger<EventHub> logger) : Hub<IEventHub>
{
    public async Task JoinGroup(string eventIdAsString)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, eventIdAsString);
    }

    public async Task AddVisitorToEventAsync(Voter loggedVoter, 
        string eventIdAsString)
    {
        try
        {
            var voterId = loggedVoter.Id;
            var eventIdAsGuid = Guid.Parse(eventIdAsString);
            
            var result = await eventService.AddVoterToEventAsync(voterId, eventIdAsGuid);
            
            if (!result.IsSuccess)
            {
                await eventNotifier.BroadcastErrorReponseToClient(Context.ConnectionId, result.Error);
                return; 
            }
            
            await eventNotifier.BroadcastEventsVisitorAdded(eventIdAsString, result.Value);
        }
        catch (Exception ex)
        {
            logger.LogError("Failed to process hub operation due to {ExceptionMessage} {Exception}",
                ex.Message, ex);
            await eventNotifier.BroadcastErrorReponseToClient(Context.ConnectionId, ApplicationErrors.ApplicationExceptionError);
        }
    }
}

