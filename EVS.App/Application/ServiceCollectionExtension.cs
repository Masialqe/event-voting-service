using EVS.App.Application.UseCases.Events.CreateEvent;
using EVS.App.Application.UseCases.Events.EventHubs;
using EVS.App.Application.UseCases.Events.GetEventById;
using EVS.App.Application.UseCases.Events.JoinEvent;
using EVS.App.Application.UseCases.Events.ListEvents;
using EVS.App.Application.UseCases.Voters.ConfirmVoterEmail;
using EVS.App.Application.UseCases.Voters.CreateVoter;
using EVS.App.Application.UseCases.Voters.GetVoter;
using EVS.App.Application.UseCases.Voters.LoginVoter;
using EVS.App.Domain.Events;
using EVS.App.Domain.Voters;
using EVS.App.Infrastructure.Database.Configurations;
using EVS.App.Infrastructure.Notifiers;

namespace EVS.App.Application;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        //voters
        services.AddScoped<CreateVoterHandler>();
        services.AddScoped<ConfirmVoterEmailHandler>();
        services.AddScoped<LoginVoterHandler>();
        services.AddScoped<GetLoggedVoterHandler>();
        services.AddScoped<GetVoterByIdHandler>();
        
        //events
        services.AddScoped<CreateEventHandler>();
        services.AddScoped<LoadEventsHandler>();
        services.AddScoped<JoinEventHandler>();
        services.AddScoped<GetEventByIdHandler>();
        services.UseSignalR();
        
        return services;
    }

    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<VoterService>();
        services.AddScoped<EventService>();
        
        return services;
    }
    
    public static WebApplication MapHubs(this WebApplication app)
    {
        app.MapHub<EventHub>(HubConfig.EVENT_HUB_URL);
        return app;
    }
    
    private static IServiceCollection UseSignalR(this IServiceCollection services)
    {
        services.AddSignalR(options =>
        {
            options.ClientTimeoutInterval = TimeSpan.FromSeconds(HubConfig.HUB_TIMEOUT_SECONDS);
            options.MaximumReceiveMessageSize = HubConfig.HUB_MAX_MESSAGE_SIZE;
            options.EnableDetailedErrors = HubConfig.HUB_ENABLE_DETAILED_ERRORS;
        });

        return services;
    }
}