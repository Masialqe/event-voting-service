using EVS.App.Domain.Events;
using EVS.App.Domain.Voters;

namespace EVS.App.Domain.VoterEvents;

public sealed class VoterEvent
{
    public VoterEvent(){}

    private VoterEvent(Guid eventId, Guid voterId)
    {
        EventId = eventId;
        VoterId = voterId;
        Score = 0;
        HasVoted = false;
    }
    
    public static VoterEvent Create(Guid eventId, Guid voterId)
        => new VoterEvent(eventId, voterId); 
    
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid VoterId { get; private set; }
    public Voter? Voter { get; set; }
    
    public Guid EventId { get; private set; }
    public Event? Event { get; set; }
    
    public int Score { get; private set; }
    public bool HasVoted { get; private set; }
    
    public int GetScore() => Score;
    public void SetHasVoted() => HasVoted = true;
    public void SetScore(int score)
    {
        if(score < 0)
            throw new ArgumentException("Score cannot be negative");
        Score += score;
    }
}