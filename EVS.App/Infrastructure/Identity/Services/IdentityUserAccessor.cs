using EVS.App.Components.Account;
using EVS.App.Infrastructure.Identity.Users;
using Microsoft.AspNetCore.Identity;

namespace EVS.App.Infrastructure.Identity.Services;

internal sealed class IdentityUserAccessor(
    UserManager<VoterIdentity> userManager,
    IdentityRedirectManager redirectManager)
{
    public async Task<VoterIdentity> GetRequiredUserAsync(HttpContext context)
    {
        var user = await userManager.GetUserAsync(context.User);

        if (user is null)
        {
            redirectManager.RedirectToWithStatus("Account/InvalidUser",
                $"Error: Unable to load user with ID '{userManager.GetUserId(context.User)}'.", context);
        }

        return user;
    }
}