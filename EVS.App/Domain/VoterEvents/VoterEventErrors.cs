using EVS.App.Domain.Abstractions;

namespace EVS.App.Domain.VoterEvents;

public static class VoterEventErrors
{
    public static Error VoterEventNotFoundError
        => new Error(nameof(VoterEventNotFoundError), "The voter event was not found.");

    public static Error VoterAlreadyVotedError
        => new Error(nameof(VoterAlreadyVotedError), "The voter has been already voted.");
    
    
    public static Error VoterEventStateCannotBeProcessedError
        => new Error(nameof(VoterAlreadyVotedError), "Voting state cannot be process.");
}