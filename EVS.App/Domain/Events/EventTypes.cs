namespace EVS.App.Domain.Events;

/// <summary>
/// Available event types.
/// </summary>
public enum EventTypes
{
    /// <summary>
    /// Voting based on adding certain amount of points per each voter.
    /// </summary>
    ScaleVote = 0,
    
    /// <summary>
    /// Voting based on choosing one voter amount the others.
    /// </summary>
    SingleVote = 1,
}
