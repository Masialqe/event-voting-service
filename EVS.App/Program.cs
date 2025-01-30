using System.Text.Json;
using EVS.App.Application;
using EVS.App.Components;
using EVS.App.Infrastructure.Database;
using EVS.App.Infrastructure.Database.Context;
using EVS.App.Infrastructure.Database.Extensions;
using EVS.App.Infrastructure.Identity;
using EVS.App.Infrastructure.Messaging;
using EVS.App.Infrastructure.Messaging.Configuration;
using EVS.App.Infrastructure.Notifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

//Infrastructure
builder.Services
    .AddIdentityServices(builder.Configuration)
    .AddMessagingServices(builder.Configuration)
    .AddDatabaseServices(builder.Configuration)
    .AddNotifiers();

//Application
builder.Services
    .AddDomainServices()
    .AddApplicationServices();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapHubs();
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

//TODO enable database configuration
//await app.ConfigureDatabaseAsync();

app.Run();