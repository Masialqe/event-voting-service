using EVS.App.Domain.Abstractions;

namespace EVS.App.Domain.Exceptions;

public static class DomainErrors
{
    public static Error NoElementToProcessError 
        => new Error(nameof(NoElementToProcessError), "There is nothing to process.");
}