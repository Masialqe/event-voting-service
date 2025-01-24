using EVS.App.Application.UseCases.Voters.ConfirmVoterEmail;
using EVS.App.Application.UseCases.Voters.CreateVoter;
using EVS.App.Application.UseCases.Voters.LoginVoter;
using EVS.App.Domain.Voters;

namespace EVS.App.Application;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<CreateVoterHandler>();
        services.AddScoped<ConfirmVoterEmailHandler>();
        services.AddScoped<LoginVoterHandler>();
        
        return services;
    }

    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<VoterService>();
        
        return services;
    }
}