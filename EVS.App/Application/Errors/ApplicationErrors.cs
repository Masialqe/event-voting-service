using EVS.App.Domain.Abstractions;

namespace EVS.App.Application.Errors;

public static class ApplicationErrors
{
    public static Error ApplicationExceptionError => 
        new Error(nameof(ApplicationExceptionError), "Application exception occurred.");
    
    public static Error InvalidInputError => 
        new Error(nameof(InvalidInputError), "Invalid input data.");
}