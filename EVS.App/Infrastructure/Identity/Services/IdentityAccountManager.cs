using EVS.App.Application.Abstractions;
using EVS.App.Domain.Abstractions;

namespace EVS.App.Infrastructure.Identity.Services;

public class IdentityAccountManager : IAccountManager
{
    public async Task<Result<string>> SendAccountConfirmationMessageAsync(string userId, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Sending account confirmation message");

        return string.Empty;

    }
}