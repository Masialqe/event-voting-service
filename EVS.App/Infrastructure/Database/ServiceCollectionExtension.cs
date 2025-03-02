﻿using EVS.App.Domain.Abstractions;
using EVS.App.Domain.Abstractions.Repositories;
using EVS.App.Infrastructure.Database.Context;
using EVS.App.Infrastructure.Database.Repositories;
using EVS.App.Infrastructure.Database.Services;
using Microsoft.EntityFrameworkCore;

namespace EVS.App.Infrastructure.Database;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        
        services.AddDbContextFactory<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));
        
        services.AddScoped<IVoterRepository, VoterRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IVoterEventRepository, VoterEventRepository>();

        services.AddScoped<ITransactionService, TransactionService>();
        
        return services;
    }
}