using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using EPTools.Blazor;
using EPTools.Blazor.Services;
using EPTools.Core.Interfaces;
using EPTools.Core.Services;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("EPClient", client => 
{
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
});
builder.Services.AddSingleton<IFetchService, HttpFetchService>();
builder.Services.AddSingleton<EpDataService>();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<StatBlockTemplateService>();
builder.Services.AddScoped<EgoService>();
builder.Services.AddScoped<DiscordWebhookService>();
builder.Services.AddScoped<LifepathService>();
builder.Services.AddSingleton<AppDataService>();
builder.Services.AddMudServices();
await builder.Build().RunAsync();