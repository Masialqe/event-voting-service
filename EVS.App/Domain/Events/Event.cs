using EVS.App.Domain.VoterEvents;

namespace EVS.App.Domain.Events;

public sealed class Event
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; } = string.Empty;
    public string Description { get;} = string.Empty;
    
    public ICollection<VoterEvent> VoterEvents { get; set; } = [];
}