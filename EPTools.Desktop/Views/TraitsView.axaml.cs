using Avalonia.Controls;
using Avalonia.Interactivity;
using EPTools.Desktop.ViewModels;

namespace EPTools.Desktop.Views;

public partial class TraitsView : UserControl
{
    public TraitsView()
    {
        InitializeComponent();
    }

    private MainWindowViewModel? ViewModel => DataContext as MainWindowViewModel;

    private void AddTrait_Click(object? sender, RoutedEventArgs e)
    {
        ViewModel?.AddEgoTrait();
    }

    private void DeleteTrait_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button { DataContext: EgoTraitViewModel traitVm })
        {
            ViewModel?.DeleteEgoTrait(traitVm);
        }
    }
}
