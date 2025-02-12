namespace EVS.App.Domain.Exceptions;

public class EventNotFoundException : Exception
{
    public EventNotFoundException(string message) : base(message) { }
}
    
public class VoterNotFoundException : Exception
{
    public VoterNotFoundException(string message) : base(message) { }
}

public class VoterAlreadySignedException : Exception
{
    public VoterAlreadySignedException(string message) : base(message) { }
}