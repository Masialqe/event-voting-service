using Microsoft.AspNetCore.Identity;
using EVS.App.Infrastructure.Identity.Users;

namespace EVS.App.Components.Account;

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