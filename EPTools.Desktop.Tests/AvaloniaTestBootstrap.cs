using Avalonia;
using Avalonia.Headless;
using Avalonia.Themes.Fluent;

[assembly: AvaloniaTestApplication(typeof(EpTools.Desktop.Tests.TestAppBuilder))]

namespace EpTools.Desktop.Tests;

/// <summary>
/// Minimal test application that doesn't require DI or data loading.
/// </summary>
public class TestApp : Application
{
    public override void Initialize()
    {
        Styles.Add(new FluentTheme());
    }
}

public class TestAppBuilder
{
    public static AppBuilder BuildAvaloniaApp() =>
        AppBuilder.Configure<TestApp>()
            .UseHeadless(new AvaloniaHeadlessPlatformOptions());
}