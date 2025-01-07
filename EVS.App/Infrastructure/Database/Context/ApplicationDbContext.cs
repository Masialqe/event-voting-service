using EVS.App.Domain.Voters;
using Microsoft.EntityFrameworkCore;

namespace EVS.App.Infrastructure.Database.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Voter> Voters { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly, ConfigurationFilter);
        base.OnModelCreating(modelBuilder);
    }
    
    private static bool ConfigurationFilter(Type type) =>
        type.FullName?.Contains("Infrastructure.Database.Configurations") ?? false;
}