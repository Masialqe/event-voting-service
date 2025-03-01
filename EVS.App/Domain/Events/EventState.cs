namespace EVS.App.Domain.Events;

/// <summary>
/// Represents an event state (progress).
/// </summary>
public enum EventState
{
    /// <summary>
    /// Event has been created but not yet started.
    /// </summary>
    Created = 0,
    /// <summary>
    /// Event has been started.
    /// </summary>
    Started = 1,
    /// <summary>
    /// Event has been ended. No operations can be performed.
    /// </summary>
    Ended = 2
}