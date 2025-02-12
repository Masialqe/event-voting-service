using EVS.App.Domain.Abstractions;

namespace EVS.App.Domain.Voters;

public static class VoterErrors
{
    public static Error VoterNotCreatedError(string? details = "Cannot create a voter.") 
        => new Error(nameof(VoterNotCreatedError), details);
    public static Error VoterAlreadyExistsError => new Error(nameof(VoterAlreadyExistsError), "Voter is not unique.");
    public static Error VoterNotFoundError => new Error(nameof(VoterAlreadyExistsError), "Voter does not exist.");
    public static Error VoterOperationError => new Error(nameof(VoterOperationError), "Error occured during processing operation.");
}