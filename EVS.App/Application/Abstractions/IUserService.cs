using EVS.App.Domain.Abstractions;

namespace EVS.App.Application.Abstractions;

/// <summary>
/// 
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Create application user.
    /// </summary>
    /// <param name="username"> Unique username. </param>
    /// <param name="email"> Unique email. </param>
    /// <param name="password"> User's password. </param>
    /// <returns> Application's user ID as string.</returns>
    Task<Result<string>> CreateUserAsync(
        string username,
        string email,
        string password);
}