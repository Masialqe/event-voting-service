using EVS.App.Domain.Abstractions;

namespace EVS.App.Application.Abstractions;

public interface IAccountManager
{
    /// <summary>
    /// Send confirmation email for created user.
    /// </summary>
    /// <param name="userEmail"> User ID as string. </param>
    /// <param name="cancellationToken"> Optional cancellation token. </param>
    /// <returns> Callback URL as string. </returns>
    Task SendAccountConfirmationMessageAsync(string userEmail, string confirmationUrl, CancellationToken cancellationToken = default);
    Task SendPasswordResetMessageAsync(string userEmail, string passwordResetUrl, CancellationToken cancellationToken = default);
    Task<Result> ConfirmEmailAsync(string userEmail, string code, CancellationToken cancellationToken = default);
    
    //Task<Result> BlockUserAsync(string userId, CancellationToken cancellationToken = default);
}