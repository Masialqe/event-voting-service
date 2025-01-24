using System.ComponentModel.DataAnnotations;
using EVS.App.Shared.Extensions;

namespace EVS.App.Infrastructure.Messaging.Configuration;

/// <summary>
/// Represents the configuration options for the SMTP client used for sending emails.
/// </summary>
public sealed class SmtpOptions : IAppOptions
{
    public static readonly string SectionName = "SmtpOptions";

    /// <summary>
    /// SMTP Host name.
    /// </summary>
    /// <example>smtp.example.com</example>
    [Required]
    public string Host { get; set; } = string.Empty;

    /// <summary>
    /// SMTP target port.
    /// </summary>
    /// <example>587</example>
    [Required]
    public int Port { get; set; } = 465;

    /// <summary>
    /// SMTP Username to authenticate.
    /// </summary>
    /// <example>user@example.com</example>
    [Required]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// SMTP User's password to authenticate.
    /// </summary>
    [Required]
    public string Password { get; set; } = string.Empty;
    
    /// <summary>
    /// Custom sender name. Default - app name.
    /// </summary>
    public string Name {  get; set; } = nameof(EVS.App);
    
}