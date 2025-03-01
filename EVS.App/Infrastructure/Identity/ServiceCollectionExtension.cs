using EVS.App.Application.Abstractions;
using EVS.App.Components.Account;
using EVS.App.Infrastructure.Identity.Database;
using EVS.App.Infrastructure.Identity.Implementations;
using EVS.App.Infrastructure.Identity.Services;
using EVS.App.Infrastructure.Identity.Users;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EVS.App.Infrastructure.Identity;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddCascadingAuthenticationState();
        services.AddScoped<IdentityUserAccessor>();
        services.AddScoped<IdentityRedirectManager>();
        services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

        services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies();

        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        services.AddDbContext<IdentityDbContext>(options =>
            options.UseNpgsql(connectionString));
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddIdentityCore<VoterIdentity>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        services.AddSingleton<IEmailSender<VoterIdentity>, IdentityEmailSender>();
        services.AddScoped<IUserService, IdentityUserService>();
        services.AddScoped<IAccountManager, IdentityAccountManager>();
        services.AddScoped<IVoterAccessor, IdentityVoterAccessor>();
        
        //Store key inside docker container.
        services.AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo("/app/appData"))
            .SetApplicationName(nameof(EVS.App));
        
        return services;
    }
}