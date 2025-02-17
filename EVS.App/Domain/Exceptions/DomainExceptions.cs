namespace EVS.App.Domain.Exceptions;

public class DomainException(string message) : Exception(message);


public class EventNotFoundException(string message) : DomainException(message);
    
public class VoterNotFoundException(string message) : DomainException(message);

public class VoterAlreadySignedException(string message) : DomainException(message);

public class VoterLimitReachedException(string message) : DomainException(message);