using EVS.App.Shared.Extensions;

namespace EVS.App.Infrastructure.Messaging.Configuration;

public sealed class RabbitMqOptions : IAppOptions
{
    public static readonly string SectionName = "RabbitMQ";
    
    public string Host { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}