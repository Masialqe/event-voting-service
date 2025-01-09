using EVS.App.Domain.Abstractions;

namespace EVS.App.Domain.Voters;

public static class VoterErrors
{
    public static Error VoterNotCreatedError => new Error(nameof(VoterNotCreatedError), "Cannot create voter.");
    public static Error VoterAlreadyExistsError => new Error(nameof(VoterAlreadyExistsError), "Voter is not unique.");
    public static Error VoterDoesntExistsError => new Error(nameof(VoterAlreadyExistsError), "Voter does not exist.");
    public static Error VoterOperationError => new Error(nameof(VoterOperationError), "Cannot perform operation.");
}