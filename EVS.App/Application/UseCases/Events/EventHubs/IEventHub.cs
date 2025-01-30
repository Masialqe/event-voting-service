using EVS.App.Domain.Abstractions;
using EVS.App.Domain.VoterEvents;

namespace EVS.App.Application.UseCases.Events.EventHubs;

public interface IEventHub
{
    Task EventStarted();
    Task EventEnded();
    Task EventRestarted();
    Task ErrorOccured(Error error);
    Task VoterAdded(VoterEvent voterEvent);
}