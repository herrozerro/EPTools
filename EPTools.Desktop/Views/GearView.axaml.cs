using Avalonia.Controls;
using Avalonia.Interactivity;
using EPTools.Desktop.ViewModels;

namespace EPTools.Desktop.Views;

public partial class GearView : UserControl
{
    public GearView()
    {
        InitializeComponent();
    }

    private MainWindowViewModel? ViewModel => DataContext as MainWindowViewModel;

    private void AddInventoryItem_Click(object? sender, RoutedEventArgs e)
    {
        ViewModel?.AddInventoryItem();
    }

    private void DeleteInventoryItem_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button { DataContext: InventoryItemViewModel itemVm })
        {
            ViewModel?.DeleteInventoryItem(itemVm);
        }
    }

    private void MoveToCache_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button { DataContext: InventoryItemViewModel itemVm } &&
            ViewModel?.SelectedCache != null)
        {
            ViewModel.MoveItemToCache(itemVm, ViewModel.SelectedCache);
        }
    }

    private void AddCache_Click(object? sender, RoutedEventArgs e)
    {
        ViewModel?.AddCache();
    }

    private void DeleteCache_Click(object? sender, RoutedEventArgs e)
    {
        if (ViewModel?.SelectedCache != null)
        {
            ViewModel.DeleteCache(ViewModel.SelectedCache);
        }
    }

    private void AddItemToCache_Click(object? sender, RoutedEventArgs e)
    {
        if (ViewModel?.SelectedCache != null)
        {
            ViewModel.AddItemToCache(ViewModel.SelectedCache);
        }
    }

    private void DeleteCacheItem_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button { DataContext: InventoryItemViewModel itemVm } &&
            ViewModel?.SelectedCache != null)
        {
            ViewModel.DeleteItemFromCache(ViewModel.SelectedCache, itemVm);
        }
    }

    private void MoveFromCache_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button { DataContext: InventoryItemViewModel itemVm } &&
            ViewModel?.SelectedCache != null)
        {
            ViewModel.MoveItemFromCache(itemVm, ViewModel.SelectedCache);
        }
    }
}
