using EVS.App.Domain.Abstractions;

namespace EVS.App.Infrastructure.Identity.Errors;

public static class IdentityErrors
{
    public static Error InvalidHttpContextError 
        => new Error(nameof(InvalidHttpContextError), "The context is invalid.");
    
    public static Error UserNotFoundError 
        => new Error(nameof(UserNotFoundError), "Logged user not found.");
}