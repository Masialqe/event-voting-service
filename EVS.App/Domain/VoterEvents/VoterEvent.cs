using EVS.App.Domain.Events;
using EVS.App.Domain.Voters;

namespace EVS.App.Domain.VoterEvents;

public sealed class VoterEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid VoterId { get; set; }
    public Voter? Voter { get; set; }
    
    public Guid EventId { get; set; }
    public Event? Event { get; set; }
    
    public int Score { get; set; }
    public bool HasVoted { get; set; }
}