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
        => await hubContext.Clients.Groups(groupName).EventStarted();

    public async Task BroadcastEventEnded(string groupName) 
        => await hubContext.Clients.Groups(groupName).EventEnded();

    public async Task BroadcastEventsVisitorAdded(string groupName, Guid voterEventId) 
        => await hubContext.Clients.Groups(groupName).VoterAdded(voterEventId);

    public async Task ReturnErrorReponseToClient(string connectionId, Error error) 
        => await hubContext.Clients.Client(connectionId).ErrorOccured(error);

    public async Task VisitorAddedSuccessfullyResponse(string connectionId, string eventId) 
        => await hubContext.Clients.Client(connectionId).VoterAddedSuccessfully(eventId);
}