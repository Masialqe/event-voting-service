using EVS.App.Application.Abstractions;
using EVS.App.Application.Errors;
using EVS.App.Domain.Abstractions;

namespace EVS.App.Application.UseCases.Voters.ResendVoterConfirmation;

public class ResendVoterConfirmationHandler(
    IAccountManager accountManager,
    ILogger<ResendVoterConfirmationHandler> logger) : IHandler<Result, ResendVoterConfirmationRequest>
{
    public async Task<Result> Handle(ResendVoterConfirmationRequest request, 
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.ConfirmationUrl))
            return ApplicationErrors.InvalidInputError;

        try
        {
            await accountManager.SendAccountConfirmationMessageAsync(request.Email, request.ConfirmationUrl, cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError("Failed to process operation for email {UserEmail} due to {ExceptionMessage} {Exception}",
                request.Email, ex.Message, ex);
            return ApplicationErrors.ApplicationExceptionError;
        }
    }
}