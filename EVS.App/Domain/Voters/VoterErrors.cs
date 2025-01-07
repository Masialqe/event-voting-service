using EVS.App.Domain.Abstractions;

namespace EVS.App.Domain.Voters;

public static class VoterErrors
{
    public static Error VoterNotCreatedError => new Error(nameof(VoterNotCreatedError), "Cannot create voter.");
    public static Error VoterAlreadyExistsError => new Error(nameof(VoterAlreadyExistsError), "Voter is not unique.");
}