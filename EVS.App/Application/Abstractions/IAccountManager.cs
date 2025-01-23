using EVS.App.Domain.Abstractions;

namespace EVS.App.Application.Abstractions;

public interface IAccountManager
{
    Task SendAccountConfirmationMessageAsync(string userEmail, string confirmationUrl, CancellationToken cancellationToken = default);
    Task SendPasswordResetMessageAsync(string userEmail, string passwordResetUrl, CancellationToken cancellationToken = default);
    Task<Result> ConfirmEmailAsync(string userId, string code, CancellationToken cancellationToken = default);
    Task<Result> LoginAsync(string userEmail, string password, bool rememberMe, CancellationToken cancellationToken = default);
    
    //Task<Result> BlockUserAsync(string userId, CancellationToken cancellationToken = default);
}