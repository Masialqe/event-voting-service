using System.Text;
using System.Text.Encodings.Web;
using EVS.App.Application.Abstractions;
using EVS.App.Infrastructure.Identity.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace EVS.App.Infrastructure.Identity.Implementations;

public class IdentityAccountManager(
    IEmailSender<VoterIdentity> emailSender,
    UserManager<VoterIdentity> userManager) : IAccountManager
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
    
    private string BuildConfirmationUrl(string confirmationUrl, string userId, string code)
    {
        var builder = new UriBuilder(confirmationUrl)
        {
            Query = $"userId={Uri.EscapeDataString(userId)}&code={Uri.EscapeDataString(code)}"
        };
        
        return builder.ToString();
    }
}