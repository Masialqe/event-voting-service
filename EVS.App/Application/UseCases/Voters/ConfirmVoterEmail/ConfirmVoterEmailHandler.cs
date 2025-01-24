using EVS.App.Application.Abstractions;
using EVS.App.Application.Errors;
using EVS.App.Domain.Abstractions;

namespace EVS.App.Application.UseCases.Voters.ConfirmVoterEmail;

public class ConfirmVoterEmailHandler(
    IAccountManager accountManager,
    ILogger<ConfirmVoterEmailHandler> logger) : IHandler<Result, ConfirmVoterEmailRequest>
{
    public async Task<Result> Handle(ConfirmVoterEmailRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(request.UserId) ||
            string.IsNullOrEmpty(request.Code))
            return ApplicationErrors.InvalidInputError;
        
        try
        {
            var result = await accountManager.ConfirmEmailAsync(request.UserId, request.Code, cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError("Failed to process operation for email {UserEmail} due to {ExceptionMessage} {Exception}",
                request.UserId, ex.Message, ex);
            return ApplicationErrors.ApplicationExceptionError;
        }
    }
}