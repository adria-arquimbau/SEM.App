using System.Security.Claims;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SEM.App;
using SEM.App.Authentication;
using SEM.App.Data;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton<HttpClient>();
builder.Services.AddSingleton<AuthService>();
builder.Services.AddSingleton<EventsService>();
builder.Services.AddSingleton<UsersService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

builder.Services.AddBlazoredSessionStorage();
builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy("admin-organizer", policy => policy.RequireClaim(ClaimTypes.Role, RoleConstants.Admin, RoleConstants.Organizer));
    options.AddPolicy("admin-organizer-user", policy => policy.RequireClaim(ClaimTypes.Role, RoleConstants.Admin, RoleConstants.Organizer, RoleConstants.User));
    options.AddPolicy("admin", policy => policy.RequireClaim(ClaimTypes.Role, RoleConstants.Admin));
    
});

await builder.Build().RunAsync();
