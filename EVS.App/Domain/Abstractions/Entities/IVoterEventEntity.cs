using EVS.App.Domain.VoterEvents;

namespace EVS.App.Domain.Abstractions.Entities;

public interface IVoterEventEntity : IEntity
{
    ICollection<VoterEvent> VoterEvents { get; }
    void AddVoterEvent(VoterEvent voterEvent);
}