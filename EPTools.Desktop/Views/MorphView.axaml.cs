using Avalonia.Controls;
using Avalonia.Interactivity;
using EPTools.Desktop.ViewModels;

namespace EPTools.Desktop.Views;

public partial class MorphView : UserControl
{
    public MorphView()
    {
        InitializeComponent();
    }

    private MainWindowViewModel? ViewModel => DataContext as MainWindowViewModel;

    private void AddMorph_Click(object? sender, RoutedEventArgs e)
    {
        ViewModel?.AddMorph();
    }

    private void DeleteMorph_Click(object? sender, RoutedEventArgs e)
    {
        if (ViewModel?.SelectedMorph != null)
        {
            ViewModel.DeleteMorph(ViewModel.SelectedMorph);
        }
    }

    private void SetActiveMorph_Click(object? sender, RoutedEventArgs e)
    {
        if (ViewModel?.SelectedMorph != null)
        {
            ViewModel.SetActiveMorph(ViewModel.SelectedMorph);
        }
    }

    private void AddTrait_Click(object? sender, RoutedEventArgs e)
    {
        ViewModel?.SelectedMorph?.AddTrait();
    }

    private void DeleteTrait_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button { DataContext: MorphTraitViewModel traitVm })
        {
            ViewModel?.SelectedMorph?.RemoveTrait(traitVm);
        }
    }

    private void AddWare_Click(object? sender, RoutedEventArgs e)
    {
        ViewModel?.SelectedMorph?.AddWare();
    }

    private void DeleteWare_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button { DataContext: MorphWareViewModel wareVm })
        {
            ViewModel?.SelectedMorph?.RemoveWare(wareVm);
        }
    }
}
