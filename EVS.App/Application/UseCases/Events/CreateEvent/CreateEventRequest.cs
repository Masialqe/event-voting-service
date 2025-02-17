using EVS.App.Domain.Events;

namespace EVS.App.Application.UseCases.Events.CreateEvent;

public sealed record CreateEventRequest(string EventName, string EventDescription, 
    EventTypes EventTypes, int PlayerLimit)
{
    public static CreateEventRequest Create(string eventName, string eventDescription, EventTypes eventTypes, int playerLimit) 
        => new CreateEventRequest(eventName, eventDescription, eventTypes, playerLimit);
};
