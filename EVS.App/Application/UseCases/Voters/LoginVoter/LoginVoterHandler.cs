using EVS.App.Application.Abstractions;
using EVS.App.Application.Errors;
using EVS.App.Domain.Abstractions;

namespace EVS.App.Application.UseCases.Voters.LoginVoter;

public class LoginVoterHandler(
    IAccountManager accountManager,
    ILogger<LoginVoterHandler> logger) : IHandler<Result, LoginVoterRequest>
{
    public async Task<Result> Handle(LoginVoterRequest request, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await accountManager.LoginAsync(
                request.Email, request.Password, request.RememberMe, cancellationToken);
            
            logger.LogInformation("User {UserEmail} logged in.", request.Email);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError("Failed to process operation for {UserEmail} due to {ExceptionMessage} {Exception}",
                request.Email, ex.Message, ex);
            return ApplicationErrors.ApplicationExceptionError;
        }
    }
}