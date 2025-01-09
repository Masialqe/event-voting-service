using EVS.App.Application.Abstractions;
using EVS.App.Infrastructure.Messaging.Configuration;
using EVS.App.Application.Messaging;
using EVS.App.Shared.Abstractions;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MimeKit;

namespace EVS.App.Infrastructure.Messaging.Services;

public class SmtpMessageService(
    ILogger<SmtpMessageService> logger,
    IOptions<SmtpOptions> options) : IMessageService
{
    private readonly SmtpOptions _smtpOptions = options.Value;
    
    /// <summary>
    /// Asynchronously sends an email message using the provided SMTP settings and message details.
    /// </summary>
    /// <param name="message">The <see cref="Message"/> object that contains the email details, such as recipient, subject, body, etc.</param>
    /// <param name="cancellationToken">An optional <see cref="CancellationToken"/> to observe while sending the email. The operation can be cancelled if requested.</param>
    /// <returns></returns>
    /// <remarks>
    /// This method creates a new SMTP connection using the provided settings, sends the email, and disconnects the client after sending. 
    /// If an error occurs during the process, it logs the exception message. 
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="message"/> is null.</exception>
    public async Task SendAsync(Message message, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message);
        
        var emailMessage = CreateNewEmailMessage(message);

        using var client = new SmtpClient();
        
        try
        {
            await client.CreateConnection(_smtpOptions, cancellationToken);
            await client.SendAsync(emailMessage, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError("Failed to send message - {Message}. {ExceptionMessage} {Exception}",
                message, ex.Message, ex);
            throw;
        }
        finally
        {
            await client.DisconnectAsync(true, cancellationToken);
        }
    }
    
    /// <summary>
    /// Creates new instance of <see cref="MimeMessage"/> based on <see cref="Message"/> object.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    private MimeMessage CreateNewEmailMessage(Message message)
    {
        var newMessage = new MimeMessage();
        
        newMessage.From.Add(new MailboxAddress(_smtpOptions.Name, _smtpOptions.UserName));
        newMessage.To.Add(MailboxAddress.Parse(message.Receiver));
        newMessage.Subject = message.Subject;
        newMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = message.Content
        };
        
        return newMessage;
    }
    
}
public static class MessageServiceExtensions
{
    /// <summary>
    /// Establish authorized connection to the mail server.
    /// </summary>
    /// <param name="smtpClient"></param>
    /// <param name="smtpOptions"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<SmtpClient> CreateConnection(this SmtpClient smtpClient, 
        SmtpOptions smtpOptions, CancellationToken cancellationToken = default)
    {
        await smtpClient.ConnectAsync(smtpOptions.Host, smtpOptions.Port, true, cancellationToken);
        await smtpClient.AuthenticateAsync(smtpOptions.UserName, smtpOptions.Password, cancellationToken);

        return smtpClient;
    }
}