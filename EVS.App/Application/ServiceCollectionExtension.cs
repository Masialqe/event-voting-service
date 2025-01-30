using EVS.App.Application.UseCases.Events.CreateEvent;
using EVS.App.Application.UseCases.Events.EventHubs;
using EVS.App.Application.UseCases.Events.ListEvents;
using EVS.App.Application.UseCases.Voters.ConfirmVoterEmail;
using EVS.App.Application.UseCases.Voters.CreateVoter;
using EVS.App.Application.UseCases.Voters.GetVoter;
using EVS.App.Application.UseCases.Voters.LoginVoter;
using EVS.App.Domain.Events;
using EVS.App.Domain.Voters;
using EVS.App.Infrastructure.Database.Configurations;

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
        
        //events
        services.AddScoped<CreateEventHandler>();
        services.AddScoped<LoadEventsHandler>();
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
        app.MapHub<EventHub>("/eventHub");
        return app;
    }
    
    private static IServiceCollection UseSignalR(this IServiceCollection services)
    {
        services.AddSignalR(options =>
        {
            options.ClientTimeoutInterval = TimeSpan.FromSeconds(60);
            options.MaximumReceiveMessageSize = 1024 * 1024 * 5; //5MB
        });

        return services;
    }
}