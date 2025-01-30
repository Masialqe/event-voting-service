using EVS.App.Application.UseCases.Events.EventHubs;
using EVS.App.Application.Abstractions;
using Microsoft.AspNetCore.SignalR;
using EVS.App.Domain.Abstractions;
using EVS.App.Domain.VoterEvents;

namespace EVS.App.Infrastructure.Notifiers;

public class SignalRNotifier(
    IHubContext<EventHub, IEventHub> hubContext) : IEventNotifier
{
    public async Task BroadcastEventStarted(string groupName)
    {
        await hubContext.Clients.Groups(groupName).EventStarted();
    }

    public async Task BroadcastEventEnded(string groupName)
    {
        await hubContext.Clients.Groups(groupName).EventEnded();
    }

    public async Task BroadcastEventsVisitorAdded(string groupName, VoterEvent voterEvent)
    {
        await hubContext.Clients.Groups(groupName).VoterAdded(voterEvent);
    }
    
    public async Task BroadcastErrorReponseToClient(string connectionId, Error error)
    {
        await hubContext.Clients.Client(connectionId).ErrorOccured(error);
    }
}