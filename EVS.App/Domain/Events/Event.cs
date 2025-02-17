using EVS.App.Domain.Abstractions.Entities;
using EVS.App.Domain.Exceptions;
using EVS.App.Domain.VoterEvents;
using EVS.App.Domain.Voters;

namespace EVS.App.Domain.Events;

/// <summary>
/// Event domain model.
/// </summary>
public sealed class Event : IVoterEventEntity
{
    /// <summary>
    /// Create new event instance.
    /// </summary>
    /// <param name="eventName"> Event name. </param>
    /// <param name="eventDescription"> Event description. </param>
    /// <param name="voterId"> Voter who creates event. </param>
    /// <param name="eventTypes">The vote type to set. See <see cref="EventTypes"/> for available options.</param>
    /// <param name="playerLimit"> Max players that can join event. Default: 2</param>
    /// <returns></returns>
    public static Event Create(string eventName, string eventDescription, Guid voterId, EventTypes eventTypes, int playerLimit)
        => new Event(eventName, eventDescription, voterId, eventTypes, playerLimit);

    private Event(string eventName, string eventDescription, Guid voterId, EventTypes eventType, int voterLimit)
    {
        Name = eventName;
        Description = eventDescription;
        VoterId = voterId;
        EventType = eventType;
        VoterLimit = voterLimit;
    }
    
    public Event() {}
        
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; } = string.Empty;
    public string Description { get;} = string.Empty;

    public EventState EventState { get; private set; } = EventState.Created;
    public EventTypes EventType { get; private set; }
    
    public int VoterLimit { get; private set; } = 2;
    public bool IsVoterLimitReached => VoterLimit == VoterEvents.Count;
    public int VotersCount => VoterEvents.Count;

    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    
    public Guid VoterId { get; private set; }
    public Voter? Voter { get; private set; }
    
    public ICollection<VoterEvent> VoterEvents { get; set; } = [];
    
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
        
        if(IsVoterLimitReached)
            throw new VoterLimitReachedException("Player limit reached.");
        
        if(VoterEvents.Any(voterEvent => voterEvent.VoterId == voterId))
            throw new VoterAlreadySignedException($"Voter is already signed to event {Id}.");
        
        VoterEvents.Add(voter);
    }
    
    public Dictionary<string, int> GetVotersScores()
    {
        //todo: add sorting
        var resultDict = new Dictionary<string, int>();

        foreach (var voterEvent in VoterEvents)
        {
            resultDict[voterEvent.VoterName!] = voterEvent.Score;
        }
        
        return resultDict;
    }

    public bool IsVoterSigned(Guid voterId)
        => VoterEvents.Any(voterEvent => voterEvent.VoterId == voterId);
}

public sealed record CreateEventDto(
    string EventName,
    string EventDescription,
    Guid VoterId,
    EventTypes EventTypes,
    int VoterLimit)
{
    public static CreateEventDto Create(string eventName, string eventDescription, Guid voterId, EventTypes eventTypes, int voterLimit)
    => new CreateEventDto(eventName, eventDescription, voterId, eventTypes, voterLimit);
    
    public Event ToDomain()
        => Event.Create(EventName, EventDescription, VoterId, EventTypes, VoterLimit); 
}