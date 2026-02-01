using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Interactivity;
using EpTools.Desktop.ViewModels;
using EPTools.Core.Models.Ego;

namespace EpTools.Desktop.Views;

public partial class SkillsView : UserControl
{
    public static readonly IValueConverter IsNotEgoSkillConverter = new SkillTypeConverter();

    public SkillsView()
    {
        InitializeComponent();
    }

    private MainWindowViewModel? ViewModel => DataContext as MainWindowViewModel;

    private void AddKnowledgeSkill_Click(object? sender, RoutedEventArgs e)
    {
        ViewModel?.CurrentEgo.Skills.Add(new EgoSkill
        {
            Name = "New Knowledge Skill",
            SkillType = SkillType.KnowledgeSkill,
            Aptitude = "Intuition"
        });
    }

    private void AddExoticSkill_Click(object? sender, RoutedEventArgs e)
    {
        ViewModel?.CurrentEgo.Skills.Add(new EgoSkill
        {
            Name = "New Exotic Skill",
            SkillType = SkillType.ExoticSkill,
            Aptitude = "Reflexes"
        });
    }

    private void DeleteSkill_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button { DataContext: EgoSkill skill })
        {
            ViewModel?.CurrentEgo.Skills.Remove(skill);
        }
    }

    private class SkillTypeConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is SkillType skillType && skillType != SkillType.EgoSkill;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
