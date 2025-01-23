using System.Text;
using System.Text.Encodings.Web;
using EVS.App.Application.Abstractions;
using EVS.App.Domain.Abstractions;
using EVS.App.Domain.Voters;
using EVS.App.Infrastructure.Identity.Users;
using EVS.App.Shared.Errors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace EVS.App.Infrastructure.Identity.Implementations;

//TODO: Remove code duplicates
public class IdentityAccountManager(
    IEmailSender<VoterIdentity> emailSender,
    UserManager<VoterIdentity> userManager,
    SignInManager<VoterIdentity> signInManager) : IAccountManager
{
    public async Task SendAccountConfirmationMessageAsync(string userEmail, string confirmationUrl,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(userEmail);
        ArgumentNullException.ThrowIfNull(user);
         
        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        
        var callbackUrl = BuildConfirmationUrl(confirmationUrl, user.Id, code);
        
        await emailSender.SendConfirmationLinkAsync(user, userEmail, HtmlEncoder.Default.Encode(callbackUrl));
    }

    public async Task SendPasswordResetMessageAsync(string userEmail, string passwordResetUrl,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(userEmail);
        ArgumentNullException.ThrowIfNull(user);
        
        var code = await userManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var callbackUrl = BuildConfirmationUrl(passwordResetUrl, user.Id, code);
        
        await emailSender.SendPasswordResetLinkAsync(user, userEmail, HtmlEncoder.Default.Encode(callbackUrl));
    }

    public async Task<Result> ConfirmEmailAsync(string userId, string code, 
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        
        if (user is null)
            return VoterErrors.VoterDoesntExistsError;
        
        var result = await userManager.ConfirmEmailAsync(user, code);

        return result.Succeeded ? Result.Success() : VoterErrors.VoterOperationError;
    }

    public async Task<Result> LoginAsync(string userEmail, string password, bool rememberMe,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(userEmail);
        ArgumentNullException.ThrowIfNull(user);
        
        var result = await signInManager.PasswordSignInAsync(user, password, rememberMe, lockoutOnFailure: false);

        return result switch
        {
            { Succeeded: true } => Result.Success(),
            { IsLockedOut: true } => LoginErrors.UserIsLockedOut,
            { RequiresTwoFactor: true } => LoginErrors.UserRequiresTwoFactor,
            _ => LoginErrors.ErrorLoginUser
        };
    }

    private string BuildConfirmationUrl(string confirmationUrl, string userId, string code)
    {
        var builder = new UriBuilder(confirmationUrl)
        {
            Query = $"userId={Uri.EscapeDataString(userId)}&code={Uri.EscapeDataString(code)}"
        };
        
        return builder.ToString();
    }
}