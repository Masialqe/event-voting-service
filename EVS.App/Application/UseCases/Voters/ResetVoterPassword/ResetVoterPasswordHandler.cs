using EVS.App.Application.Abstractions;
using EVS.App.Application.Errors;
using EVS.App.Domain.Abstractions;

namespace EVS.App.Application.UseCases.Voters.ResetVoterPassword;

public class ResetVoterPasswordHandler(
    IUserService userService,
    IAccountManager accountManager,
    ILogger<ResetVoterPasswordHandler> logger) : IHandler<Result, ResetVoterPasswordRequest>
{
    public async Task<Result> Handle(ResetVoterPasswordRequest request, 
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(request.Email))
            return ApplicationErrors.InvalidInputError;
        
        try
        {
            var userEmailResult = await userService.IsUserEmailExistsAsync(request.Email, cancellationToken);

            if (!userEmailResult.IsSuccess)
                return ApplicationErrors.InvalidInputError;
            
            await accountManager.SendPasswordResetMessageAsync(request.Email, request.ConfirmationUrl, cancellationToken);
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