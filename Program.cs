using EventAppClient;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Configuration.Bind("DetailedErrors", options => builder.HostEnvironment.IsDevelopment());

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7259/") });

await builder.Build().RunAsync();
