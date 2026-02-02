using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using EPTools.Core.Models.Ego;
using EPTools.Desktop.ViewModels;

namespace EPTools.Desktop.Views;

public partial class NetworkView : UserControl
{
    public NetworkView()
    {
        InitializeComponent();
    }

    private MainWindowViewModel? ViewModel => DataContext as MainWindowViewModel;

    private void AddIdentity_Click(object? sender, RoutedEventArgs e)
    {
        if (ViewModel == null) return;

        var newIdentity = new Identity
        {
            Alias = "New Cover ID",
            Location = "Unknown"
        };

        // Add to the underlying model
        ViewModel.CurrentEgo.Identities.Add(newIdentity);

        // Add wrapper to observable collection
        var identityVm = new IdentityViewModel(newIdentity);
        ViewModel.Identities.Add(identityVm);
        ViewModel.SelectedIdentity = identityVm;
    }

    private void RemoveIdentity_Click(object? sender, RoutedEventArgs e)
    {
        if (ViewModel?.SelectedIdentity == null) return;

        var identities = ViewModel.Identities;
        var currentIndex = identities.IndexOf(ViewModel.SelectedIdentity);

        // Remove from underlying model
        ViewModel.CurrentEgo.Identities.Remove(ViewModel.SelectedIdentity.Model);

        // Remove from observable collection
        identities.Remove(ViewModel.SelectedIdentity);

        // Select another identity if available
        if (identities.Count > 0)
        {
            ViewModel.SelectedIdentity = identities[Math.Min(currentIndex, identities.Count - 1)];
        }
        else
        {
            ViewModel.SelectedIdentity = null;
        }
    }
}
