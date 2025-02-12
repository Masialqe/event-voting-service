using EVS.App.Domain.Abstractions;
using EVS.App.Domain.Abstractions.Entities;
using EVS.App.Domain.Exceptions;
using EVS.App.Domain.VoterEvents;
using EVS.App.Domain.Voters;

namespace EVS.App.Domain.Events;

public sealed class Event : IVoterEventEntity
{
    public static Event Create(string eventName, string eventDescription, Guid voterId)
        => new Event(eventName, eventDescription, voterId);

    private Event(string eventName, string eventDescription, Guid voterId)
    {
        Name = eventName;
        Description = eventDescription;
        VoterId = voterId;
    }
    
    public Event() {}
        
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; } = string.Empty;
    public string Description { get;} = string.Empty;

    public EventState EventState { get; private set; } = EventState.Created;
    public EventType EventType { get; private set; }

    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    
    public Guid VoterId { get; set; }
    public Voter? Voter { get; set; }
    
    public ICollection<VoterEvent> VoterEvents { get; set; } = [];
    
    public int GetVotersCount() => VoterEvents.Count;

    public void Start()
    {
        if (EventState != EventState.Created)
            throw new InvalidOperationException("Cannot start this event.");
        EventState = EventState.Started;
    }

    public void End()
    {
        if (EventState != EventState.Started)
            throw new InvalidOperationException("Cannot end not started event.");
        EventState = EventState.Ended;
    }

    public void AddVoterEvent(VoterEvent voter)
    {
        var voterId = voter.VoterId;
        
        if(VoterEvents.Any(voterEvent => voterEvent.VoterId == voterId))
            throw new VoterAlreadySignedException($"Voter is already signed to event {Id}.");
        
        VoterEvents.Add(voter);
    }

    // public void Reset()
    // {
    //     if(EventState != EventState.Started)
    //         throw new InvalidOperationException("Cannot reset this event.");
    //     EventState = EventState.Created;
    //     VoterEvents.Clear();
    //     //todo: add logic to clear VoterEvents
    // }
}