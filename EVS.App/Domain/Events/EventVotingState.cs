namespace EVS.App.Domain.Events;

/// <summary>
/// Represents the possible voting states during the processing of user votes.
/// </summary>
public enum EventVotingState
{
    /// <summary>
    /// Not all voters assigned to the event have cast their votes.
    /// </summary>
    NotAllVoted = 0,
    
    /// <summary>
    /// All voters assigned to the event have cast their votes.
    /// </summary>
    AllVoted = 1,
}
