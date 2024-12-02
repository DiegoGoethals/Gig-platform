using Gig.Platform.Web.Components;
using Gig.Platform.Web.Interfaces;
using Gig.Platform.Web.Services;
using Gig.Platform.Web.Services.Special_services;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient<ISkillService, SkillService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7257/");
});
builder.Services.AddHttpClient<IAccountService, AccountService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7257/");
});
builder.Services.AddHttpClient<IJobService, JobService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7257/");
});
builder.Services.AddHttpClient<IApplicationService, ApplicationService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7257/");
});

builder.Services.AddScoped<ILocationService, LocationService>();

builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
