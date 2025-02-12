
using EVS.App.Domain.Abstractions;

namespace EVS.App.Domain.Events;

public static class EventErrors
{
    public static Error EventAlreadyExistError => new Error(nameof(EventAlreadyExistError), "Event is already exist");
    public static Error EventNotFoundError => new Error(nameof(EventNotFoundError), "Event not found");
    public static Error EventAlreadyCointainsVoterError => new Error(nameof(EventAlreadyCointainsVoterError), "Voter is already signed to this event.");
}