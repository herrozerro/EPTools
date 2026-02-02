using Avalonia;
using Avalonia.Controls;
using EPTools.Desktop.ViewModels;

namespace EPTools.Desktop.Views;

public partial class RepNetworkWidget : UserControl
{
    public static readonly StyledProperty<string> LabelProperty =
        AvaloniaProperty.Register<RepNetworkWidget, string>(nameof(Label), string.Empty);

    public static readonly StyledProperty<RepNetworkViewModel?> RepNetworkProperty =
        AvaloniaProperty.Register<RepNetworkWidget, RepNetworkViewModel?>(nameof(RepNetwork));

    static RepNetworkWidget()
    {
        // When RepNetwork changes, update the ContentPanel's DataContext
        RepNetworkProperty.Changed.AddClassHandler<RepNetworkWidget>((widget, args) =>
        {
            widget.UpdateContentDataContext();
        });
    }

    public string Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public RepNetworkViewModel? RepNetwork
    {
        get => GetValue(RepNetworkProperty);
        set => SetValue(RepNetworkProperty, value);
    }

    public RepNetworkWidget()
    {
        InitializeComponent();
    }

    protected override void OnLoaded(Avalonia.Interactivity.RoutedEventArgs e)
    {
        base.OnLoaded(e);
        UpdateContentDataContext();
    }

    private void UpdateContentDataContext()
    {
        if (ContentPanel != null)
        {
            ContentPanel.DataContext = RepNetwork;
        }
    }
}
