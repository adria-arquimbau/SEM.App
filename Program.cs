using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SEM.App;
using SEM.App.Data;
using SEM.App.Utilities;
using SportEventManager.Data;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton<HttpClient>();
builder.Services.AddSingleton<AuthService>();
builder.Services.AddSingleton<EventsService>();
builder.Services.AddScoped<ILocalStorage, LocalStorage>();

await builder.Build().RunAsync();