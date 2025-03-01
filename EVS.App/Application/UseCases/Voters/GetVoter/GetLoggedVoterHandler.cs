using EVS.App.Application.Abstractions;
using EVS.App.Application.Errors;
using EVS.App.Domain.Abstractions;
using EVS.App.Domain.Voters;

namespace EVS.App.Application.UseCases.Voters.GetVoter;

public sealed class GetLoggedVoterHandler(
    IVoterAccessor voterAccessor,
    ILogger<GetLoggedVoterHandler> logger) : IHandler<Result<Voter>, GetloggedVoterRequest>
{
    public async Task<Result<Voter>> Handle(GetloggedVoterRequest? request = default, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await voterAccessor.GetCurrentLoggedVoterAsync(cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError("Failed to get logged visitor due to {ExceptionMessage} {Exception}",
                ex.Message, ex);
            return ApplicationErrors.ApplicationExceptionError;
        }
    }
}