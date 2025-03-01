using EVS.App.Application.Abstractions;
using EVS.App.Application.Errors;
using EVS.App.Domain.Abstractions;
using EVS.App.Domain.Abstractions.Repositories;
using EVS.App.Domain.VoterEvents;

namespace EVS.App.Application.UseCases.Voters.GetVoterEvent;

public sealed class GetVoterEventByIdHandler(
    IVoterEventRepository voterEventRepository,
    ILogger<GetVoterEventByIdHandler> logger) : IHandler<Result<VoterEvent>, GetVoterEventByIdRequest>
{
    public async Task<Result<VoterEvent>> Handle(GetVoterEventByIdRequest request, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            //todo: consider creating service also for voterevent
            var result = await voterEventRepository.GetByIdAsync(request.VoterEventId, cancellationToken);

            if (result is null)
                return VoterEventErrors.VoterEventNotFoundError;
            
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