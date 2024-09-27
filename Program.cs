using Blazored.LocalStorage;
using EventAppClient;
using EventAppClient.Pages;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Configuration.Bind("DetailedErrors", options => builder.HostEnvironment.IsDevelopment());

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7259/") });
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddSingleton<UserState>();

builder.Services.AddBlazoredLocalStorage();


await builder.Build().RunAsync();
