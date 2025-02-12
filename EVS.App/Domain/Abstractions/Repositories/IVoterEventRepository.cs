using EVS.App.Domain.VoterEvents;

namespace EVS.App.Domain.Abstractions.Repositories;

public interface IVoterEventRepository
{
    Task LinkEventAndVoterAsync(VoterEvent voterEvent, 
        CancellationToken cancellationToken = default);
}