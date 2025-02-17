using EVS.App.Application.Abstractions;
using EVS.App.Application.Errors;
using EVS.App.Domain.Abstractions;
using EVS.App.Domain.Voters;

namespace EVS.App.Application.UseCases.Voters.GetVoter;

public class GetVoterByIdHandler(
    VoterService voterService,
    ILogger<GetVoterByIdHandler> logger) : IHandler<Result<Voter>, GetVoterByIdRequest>
{
    public async Task<Result<Voter>> Handle(GetVoterByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await voterService.GetVoterAsync(request.VoterId, cancellationToken);
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