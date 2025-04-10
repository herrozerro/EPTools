using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using EPTools.Blazor;
using EPTools.Core.Interfaces;
using EPTools.Core.Services;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<IFetchService, HttpFetchService>();
builder.Services.AddScoped<StatBlockTemplateService>();
builder.Services.AddScoped<EPDataService>();
builder.Services.AddScoped<DiscordWebhookService>();
builder.Services.AddScoped<LifepathService>();
builder.Services.AddMudServices();
await builder.Build().RunAsync();