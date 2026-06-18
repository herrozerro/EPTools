using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using System;
using Avalonia.Markup.Xaml;
using EPTools.Core.Interfaces;
using EPTools.Core.Services;
using EPTools.Desktop.ViewModels;
using EPTools.Desktop.Views;
using Microsoft.Extensions.DependencyInjection;

namespace EPTools.Desktop;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Set up dependency injection
            var services = new ServiceCollection();
            ConfigureServices(services);
            Services = services.BuildServiceProvider();

            desktop.MainWindow = new MainWindow
            {
                DataContext = Services.GetRequiredService<MainWindowViewModel>(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        // Core services
        services.AddSingleton<IFetchService, FileFetchService>();
        services.AddSingleton<IUserDataStore, FileUserDataStore>();
        services.AddSingleton<IRandomizer, DefaultRandomizer>();
        services.AddSingleton<IEpDataService, EpDataService>();
        services.AddSingleton<EgoService>();
        services.AddSingleton<LifepathService>();
        services.AddSingleton<EgoManager>();

        // ViewModels
        services.AddTransient<MainWindowViewModel>();
    }


}
