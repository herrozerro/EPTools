using Avalonia.Controls;
using Avalonia.Interactivity;
using EPTools.Desktop.ViewModels;

namespace EPTools.Desktop.Views;

public partial class SkillsView : UserControl
{
    public SkillsView()
    {
        InitializeComponent();
    }

    private MainWindowViewModel? ViewModel => DataContext as MainWindowViewModel;

    private void AddKnowledgeSkill_Click(object? sender, RoutedEventArgs e)
    {
        ViewModel?.AddKnowledgeSkill();
    }

    private void AddExoticSkill_Click(object? sender, RoutedEventArgs e)
    {
        ViewModel?.AddExoticSkill();
    }

    private void DeleteSkill_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button { DataContext: EgoSkillViewModel skillVm })
        {
            ViewModel?.DeleteSkill(skillVm);
        }
    }
}
