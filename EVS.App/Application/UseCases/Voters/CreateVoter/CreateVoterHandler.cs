using EVS.App.Application.Abstractions;
using EVS.App.Domain.Abstractions;
using EVS.App.Domain.Voters;

namespace EVS.App.Application.UseCases.Voters.CreateUser;

public class CreateVoterHandler(
    IUserService userService,
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

            if (!voterIdentityCreationResult.IsSuccess) return HandleVoterCreationError();
        
            var voterCreationResult = await voterService.CreateVoterAsync(
                new Voter(), 
                voterIdentityCreationResult.Value, 
                cancellationToken);
        
            return !voterCreationResult.IsSuccess ? HandleVoterCreationError(voterCreationResult.Error) : Result.Success();
        }
        catch (Exception e)
        {
            return HandleVoterCreationError(e);
        }
    }

    private Error HandleVoterCreationError(Error? error = null)
    {
        var resultError = error ?? VoterErrors.VoterNotCreatedError;
        
        logger.LogError("Failed to create voter due to exception. {ErrorName} {ErrorMessage}", 
            resultError.errorName, resultError.errorDescription);
        
        return resultError;
    }
}
