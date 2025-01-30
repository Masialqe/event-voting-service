using EVS.App.Domain.Abstractions;
using EVS.App.Domain.VoterEvents;

namespace EVS.App.Application.Abstractions;

public interface IEventNotifier
{
    Task BroadcastEventStarted(string groupName);
    Task BroadcastEventEnded(string groupName);
    Task BroadcastEventsVisitorAdded(string groupName, VoterEvent voterEvent);

    Task BroadcastErrorReponseToClient(string connectionId, Error error);
}