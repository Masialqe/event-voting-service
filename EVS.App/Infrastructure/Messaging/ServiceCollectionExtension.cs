using System.Net.Mail;
using EVS.App.Infrastructure.Messaging.Configuration;
using EVS.App.Infrastructure.Messaging.Queues;
using EVS.App.Infrastructure.Messaging.Services;
using EVS.App.Shared.Extensions;
using MassTransit;

namespace EVS.App.Infrastructure.Messaging;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddMessagingServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(busConfiguration =>
        {
            busConfiguration.SetKebabCaseEndpointNameFormatter();

            busConfiguration.AddMessageConsumer();
            
            //TODO: RabbitMQ configuration class
            busConfiguration.UsingRabbitMq((context, config) =>
            {
                config.Host(new Uri(configuration["RabbitMQ:Host"]!), configurator =>
                {
                    configurator.Username(configuration["RabbitMQ:User"]!);
                    configurator.Password(configuration["RabbitMQ:Password"]!);
                } );
                
                config.ConfigureEndpoints(context);
            });
        });
    
        //TODO: configuration validation
        services.AddOptions<SmtpOptions>()
            .BindConfiguration(SmtpOptions.SectionName);
        
        services.AddOptions<RabbitMqOptions>()
            .BindConfiguration(RabbitMqOptions.SectionName);
        
        //services.AddConfiguredOptions<SmtpOptions>(SmtpOptions.SectionName);
        //services.AddConfiguredOptions<RabbitMqOptions>(RabbitMqOptions.SectionName);    
        
        return services;
    }

    private static IBusRegistrationConfigurator AddMessageConsumer(this IBusRegistrationConfigurator configurator)
    {
        configurator.AddConsumer<EmailMessageQueueConsumer>(config =>
        {
            config.UseDelayedRedelivery(retry => retry.Intervals(
                TimeSpan.FromMinutes(2), 
                TimeSpan.FromMinutes(5), 
                TimeSpan.FromMinutes(8)));
            
            config.UseMessageRetry(retryConfig =>
            {
                retryConfig.Handle<SmtpException>();
                retryConfig.Interval(3, TimeSpan.FromSeconds(10));
            });
        });
        
        return configurator;
    }
}