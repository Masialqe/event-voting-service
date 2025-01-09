using EVS.App.Application.Abstractions;
using EVS.App.Domain.Abstractions;
using EVS.App.Domain.Voters;
using EVS.App.Infrastructure.Identity.Users;
using Microsoft.AspNetCore.Identity;

namespace EVS.App.Infrastructure.Identity.Services;

public class IdentityUserService(
    IUserStore<VoterIdentity> userStore,
    UserManager<VoterIdentity> userManager) : IUserService
{
    public async Task<Result<string>> CreateUserAsync(string username, string email, string password)
    {
        var user = CreateUser();

        await userStore.SetUserNameAsync(user, username, CancellationToken.None);
        var emailStore = GetEmailStore();
        await emailStore.SetEmailAsync(user, email, CancellationToken.None);
        var result = await userManager.CreateAsync(user, password);
        
        var userId = await userManager.GetUserIdAsync(user);

        return userId;
    }

    public async Task<Result<string>> IsUserEmailExistsAsync(string email, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
            return VoterErrors.VoterDoesntExistsError;
        
        return user.Email;
    }


    private VoterIdentity CreateUser()
    {
        try
        {
            return Activator.CreateInstance<VoterIdentity>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(VoterIdentity)}'. " +
                                                $"Ensure that '{nameof(VoterIdentity)}' is not an abstract class and has a parameterless constructor.");
        }
    }
    
    private IUserEmailStore<VoterIdentity> GetEmailStore()
    {
        if (!userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }

        return (IUserEmailStore<VoterIdentity>)userStore;
    }
}