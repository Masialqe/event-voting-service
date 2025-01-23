using EVS.App.Application.Abstractions;
using EVS.App.Application.UseCases.Voters.CreateUser;
using EVS.App.Domain.Abstractions;
using EVS.App.Domain.Voters;

namespace EVS.App.Application.UseCases.Voters.CreateVoter;

public class CreateVoterHandler(
    IUserService userService,
    IAccountManager accountManager,
    VoterService voterService,
    ILogger<CreateVoterHandler> logger) : IHandler<Result, CreateVoterRequest>
{
    public async Task<Result> Handle(
        CreateVoterRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var voterIdentityCreationResult = await userService.CreateUserAsync(
                request.Username,
                request.Email,
                request.Password);

            if (!voterIdentityCreationResult.IsSuccess) return HandleVoterCreationError(voterIdentityCreationResult.Error);
            
            var voterUserId = voterIdentityCreationResult.Value;
            var voter = CreateNewVoter(request.Username, voterUserId, request.Email);
            var voterCreationResult = await voterService.CreateVoterAsync(voter,
                cancellationToken);

            await accountManager.SendAccountConfirmationMessageAsync(request.Email, request.ConfirmationUrl, cancellationToken);
            
            return !voterCreationResult.IsSuccess ? HandleVoterCreationError(voterCreationResult.Error) : Result.Success();
        }
        catch (Exception ex)
        {
            return HandleVoterCreationError(ex);
        }
    }
    
    private Error HandleVoterCreationError(Error? error = null)
    {
        var resultError = error ?? VoterErrors.VoterNotCreatedError();
        
        logger.LogError("Failed to create voter due to exception. {ErrorName} {ErrorMessage}", 
            resultError.errorName, resultError.errorDescription);
        
        return resultError;
    }

    private Voter CreateNewVoter(string nickname, string userId, string email)
        => Voter.Create(nickname, userId, email);
}
