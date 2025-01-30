using EVS.App.Domain.Abstractions;
using EVS.App.Domain.Voters;

namespace EVS.App.Application.Abstractions;

public interface IVoterAccessor
{
    Task<Result<Voter>> GetCurrentLoggedVoterAsync(
        CancellationToken cancellationToken = default);
}