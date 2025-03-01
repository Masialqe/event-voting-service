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
    /// <param name="playerLimit"> Max players that can join event. Default: 1</param>
    /// <param name="availableVotingPoints">Only if <see cref="EventTypes"/> is set to Scale - max points that voter can assign. </param>
    /// <returns></returns>
    public static Event Create(string eventName, string eventDescription, Guid voterId, EventTypes eventTypes, 
        int playerLimit, int availableVotingPoints = 0)
            => new (eventName, eventDescription, voterId, eventTypes, playerLimit, availableVotingPoints);

    private Event(string eventName, string eventDescription, Guid voterId, EventTypes eventType, int voterLimit, int pointsLimit = 0)
    {
        Name = eventName;
        Description = eventDescription;
        VoterId = voterId;
        EventType = eventType;
        VoterLimit = voterLimit;
        PointsLimit = pointsLimit;
    }
    
    public Event() {}
        
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; } = string.Empty;
    public string Description { get;} = string.Empty;

    public EventState EventState { get; private set; } = EventState.Created;
    public EventTypes EventType { get; private set; }
    public int PointsLimit { get; private set; }
    
    public int VoterLimit { get; private set; } = 1;
    public int VotesCount { get; private set; } = 0;
    public int VotersCount { get; private set; }
    public bool IsVoterLimitReached => VoterLimit == VoterEvents.Count;
    public bool HasAllVotersVoted => VotesCount == VotersCount;

    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public DateTime? EndedAt { get; private set; }
    
    public Guid VoterId { get; private set; }
    public Voter? Voter { get; private set; }
    
    public ICollection<VoterEvent> VoterEvents { get; private set; } = [];
    
    /// <summary>
    /// Starts the event.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when attempting to start an event that has already been started.
    /// </exception>
    public void Start()
    {
        if (EventState != EventState.Created)
            throw new InvalidOperationException("Cannot start this event.");
    
        EventState = EventState.Started;
    }

    /// <summary>
    /// Endthe event.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when attempting to end an event that has already been ended.
    /// </exception>
    public void End()
    {
        if (EventState != EventState.Started)
            throw new InvalidOperationException("Cannot end not started event.");
        EventState = EventState.Ended;
        EndedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Adds a <see cref="VoterEvent"/> object as a voter to the event.
    /// </summary>
    /// <param name="voter">The voter event object to be added.</param>
    /// <exception cref="VoterLimitReachedException">
    /// Thrown when the voter limit for the event has been reached.
    /// </exception>
    /// <exception cref="VoterAlreadySignedException">
    /// Thrown when the voter is already signed up for the event.
    /// </exception>
    public void AddVoterEvent(VoterEvent voter)
    {
        var voterId = voter.VoterId;

        if (IsVoterLimitReached)
            throw new VoterLimitReachedException("Voter limit reached.");

        if (VoterEvents.Any(voterEvent => voterEvent.VoterId == voterId))
            throw new VoterAlreadySignedException($"Voter is already signed up for event {Id}.");

        VoterEvents.Add(voter);
        VotersCount++;
    }

    /// <summary>
    /// Retrieves the scores of voters, sorted in descending order.
    /// </summary>
    /// <returns>
    /// An array of <see cref="VoterScoreDto"/> objects, where each entry contains the voter's username and score.
    /// </returns>
    public VoterScoreDto[] GetVotersScores()
    {
        var sortedVoterEvents = VoterEvents.Select(x 
                => new VoterScoreDto(x.Voter?.Username, x.Score))
                    .OrderByDescending(s => s.Score).ToArray();
        
        return sortedVoterEvents;
    }

    /// <summary>
    /// Checks if a voter is already signed up for the event.
    /// </summary>
    /// <param name="voterId">The unique identifier of the voter.</param>
    /// <returns>
    /// <c>true</c> if the voter is signed up for the event; otherwise, <c>false</c>.
    /// </returns>
    public bool IsVoterSigned(Guid voterId)
        => VoterEvents.Any(voterEvent => voterEvent.VoterId == voterId);

    /// <summary>
    /// Increments the vote count for the event.
    /// </summary>
    /// <exception cref="VotesLimitReachedException">
    /// Thrown when the vote count reaches the voter limit.
    /// </exception>
    public void AddVote()
    {
        if (VotesCount == VoterLimit)
            throw new VotesLimitReachedException("Votes count cannot be greater than the number of voters.");
    
        VotesCount++;
    }

}

public sealed record CreateEventDto(
    string EventName,
    string EventDescription,
    Guid VoterId,
    EventTypes EventTypes,
    int VoterLimit,
    int PointsLimit = 0)
{
    public static CreateEventDto Create(string eventName, string eventDescription, Guid voterId, EventTypes eventTypes, int voterLimit, int pointsLimit = 0)
    => new CreateEventDto(eventName, eventDescription, voterId, eventTypes, voterLimit, pointsLimit);
    
    public Event ToDomain()
        => Event.Create(EventName, EventDescription, VoterId, EventTypes, VoterLimit, PointsLimit); 
}

