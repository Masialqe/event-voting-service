using EVS.App.Infrastructure.Database.Context;
using EVS.App.Infrastructure.Identity.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace EVS.App.Infrastructure.Database.Extensions;

/// <summary>
/// Provides extension methods for configuring and managing the database lifecycle during application startup.
/// </summary>
public static class DatabaseBuilderExtension
{
    /// <summary>
    /// Configures the database by ensuring it is created and up-to-date with the latest migrations.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to configure the database for.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task ConfigureDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        
        var appContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var identityContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
        
        //Configure both app and identity context.
        await ConfigureDatabaseAsync(appContext);
        await ConfigureDatabaseAsync(identityContext);
    }
    
    /// <summary>
    /// Executes database configuration for given DbContext.
    /// </summary>
    /// <param name="dbContext"></param>
    private static async Task ConfigureDatabaseAsync(DbContext dbContext)
    {
        await EnsureDatabaseCreated(dbContext);
        
        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            await ExecuteDatabaseMigrations(dbContext);
        }
    }

    /// <summary>
    /// Ensures that the database is created if it does not already exist.
    /// </summary>
    /// <param name="context">The <see cref="DbContext"/> instance used to check and create the database.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task EnsureDatabaseCreated(DbContext context)
    {
        var dbCreator = context.GetService<IRelationalDatabaseCreator>();

        var strategy = context.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            if (!await dbCreator.ExistsAsync())
                await dbCreator.CreateAsync();
        });
    }

    /// <summary>
    /// Applies pending database migrations to ensure the database schema is up-to-date.
    /// </summary>
    /// <param name="context">The <see cref="DbContext"/> instance used to apply the migrations.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task ExecuteDatabaseMigrations(DbContext context)
    {
        var strategy = context.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await context.Database.MigrateAsync();
        });
    }
}
