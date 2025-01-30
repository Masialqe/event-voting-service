namespace EVS.App.Application.UseCases.Events.CreateEvent;

public sealed record CreateEventRequest(string EventName, string EventDescription)
{
    public static CreateEventRequest Create(string eventName, string eventDescription) 
        => new CreateEventRequest(eventName, eventDescription);
};
