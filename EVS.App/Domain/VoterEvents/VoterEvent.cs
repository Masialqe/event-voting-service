using EVS.App.Domain.Abstractions;
using EVS.App.Domain.Abstractions.Entities;
using EVS.App.Domain.Events;
using EVS.App.Domain.Voters;

namespace EVS.App.Domain.VoterEvents;

public sealed class VoterEvent : IEntity
{
    public VoterEvent(){}

    private VoterEvent(Guid eventId, Guid voterId)
    {
        EventId = eventId;
        VoterId = voterId;
        
        ResetVoterEventState();
    }
    
    public static VoterEvent Create(Guid eventId, Guid voterId)
        => new VoterEvent(eventId, voterId); 
    
    public Guid Id { get; private set; } = Guid.NewGuid();
    
    public Guid VoterId { get; private set; }
    public Voter? Voter { get; private set; }
    
    public Guid EventId { get; private set; }
    public Event? Event { get; private set; }
    
    public int Score { get; private set; }
    public bool HasVoted { get; private set; }

    public byte[] RowVersion { get; private set; } = default!;

    public string? VoterName => Voter?.Username;
    
    public int GetScore() => Score;
    public void SetHasVoted() => HasVoted = true;
    public void SetScore(int score)
    {
        if(score < 0)
            throw new ArgumentException("Score cannot be negative");
        Score += score;
    }

    public void ResetVoterEventState()
    {
        Score = 0;
        HasVoted = false;
    }
}