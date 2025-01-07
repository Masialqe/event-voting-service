using EVS.App.Domain.Abstractions;

namespace EVS.App.Application.Abstractions;

public interface IAccountManager
{
    /// <summary>
    /// Send confirmation email for created user.
    /// </summary>
    /// <param name="userId"> User ID as string. </param>
    /// <param name="cancellationToken"> Optional cancellation token. </param>
    /// <returns> Callback URL as string. </returns>
    Task<Result<string>> SendAccountConfirmationMessageAsync(string userId, CancellationToken cancellationToken = default);
}