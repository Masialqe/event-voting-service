using EVS.App.Domain.Abstractions;

namespace EVS.App.Shared.Errors;

public static class LoginErrors
{
    public static Error UserIsLockedOut => new(nameof(UserIsLockedOut), "User is locked out");
    public static Error UserRequiresTwoFactor => new(nameof(UserRequiresTwoFactor), "User requires two factor authentication");
    public static Error ErrorLoginUser => new(nameof(ErrorLoginUser), "Login failed");
}