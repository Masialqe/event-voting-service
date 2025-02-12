using EVS.App.Application.Abstractions;
using EVS.App.Domain.Abstractions;
using EVS.App.Domain.Abstractions.Repositories;
using EVS.App.Domain.Voters;
using EVS.App.Infrastructure.Identity.Errors;
using EVS.App.Infrastructure.Identity.Services;

namespace EVS.App.Infrastructure.Identity.Implementations;

public class IdentityVoterAccessor(
    IVoterRepository voterRepository,
    IdentityUserAccessor identityUserAccessor,
    IHttpContextAccessor contextAccessor) : IVoterAccessor
{
    public async Task<Result<Voter>> GetCurrentLoggedVoterAsync(CancellationToken cancellationToken = default)
    {
        if (contextAccessor.HttpContext is not { } httpContext)
            return IdentityErrors.InvalidHttpContextError;

        var voterIdentity = await identityUserAccessor.GetRequiredUserAsync(httpContext);

        if (voterIdentity is null)
            return IdentityErrors.UserNotFoundError;
        
        return await voterRepository.GetVoterByUserIdAsync(voterIdentity.Id, cancellationToken);
    }
}