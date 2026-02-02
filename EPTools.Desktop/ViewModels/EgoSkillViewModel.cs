using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using EPTools.Core.Constants;
using EPTools.Core.Models.Ego;

namespace EPTools.Desktop.ViewModels;

public partial class EgoSkillViewModel : ObservableObject
{
    private readonly EgoSkill _model;
    private readonly ObservableCollection<EgoAptitudeViewModel> _aptitudes;

    public static IReadOnlyList<string> ExoticAvailableAptitudes { get; } = new[]
    {
        AptitudeNames.Cognition,
        AptitudeNames.Intuition,
        AptitudeNames.Reflexes,
        AptitudeNames.Savvy,
        AptitudeNames.Somatics,
        AptitudeNames.Willpower
    };
    
    public static IReadOnlyList<string> KnowledgeAvailableAptitudes { get; } = new[]
    {
        AptitudeNames.Cognition,
        AptitudeNames.Intuition
    };

    public EgoSkillViewModel(EgoSkill model, ObservableCollection<EgoAptitudeViewModel> aptitudes)
    {
        _model = model;
        _aptitudes = aptitudes;

        // Subscribe to aptitude changes to recalculate total
        foreach (var apt in _aptitudes)
        {
            apt.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(EgoAptitudeViewModel.AptitudeValue) &&
                    apt.Name == _model.Aptitude)
                {
                    OnPropertyChanged(nameof(Total));
                }
            };
        }
    }

    public EgoSkill Model => _model;

    public Guid Id => _model.Id;

    public string Name
    {
        get => _model.Name;
        set
        {
            if (SetProperty(_model.Name, value, _model, (m, v) => m.Name = v))
            {
                OnPropertyChanged(nameof(Total));
            }
        }
    }

    public string Specialization
    {
        get => _model.Specialization;
        set => SetProperty(_model.Specialization, value, _model, (m, v) => m.Specialization = v);
    }

    public int Rank
    {
        get => _model.Rank;
        set
        {
            if (SetProperty(_model.Rank, value, _model, (m, v) => m.Rank = v))
            {
                OnPropertyChanged(nameof(Total));
            }
        }
    }

    public string Aptitude
    {
        get => _model.Aptitude;
        set
        {
            if (SetProperty(_model.Aptitude, value, _model, (m, v) => m.Aptitude = v))
            {
                OnPropertyChanged(nameof(Total));
            }
        }
    }

    public SkillType SkillType
    {
        get => _model.SkillType;
        set => SetProperty(_model.SkillType, value, _model, (m, v) => m.SkillType = v);
    }

    public bool CanEditAptitude => SkillType != SkillType.EgoSkill;

    public bool CanDelete => SkillType != SkillType.EgoSkill;

    public int Total
    {
        get
        {
            var aptitude = _aptitudes.FirstOrDefault(a => a.Name == _model.Aptitude);
            var aptitudeValue = aptitude?.AptitudeValue ?? 0;
            return _model.Rank + aptitudeValue;
        }
    }

    public string SkillTypeDisplay => SkillType switch
    {
        SkillType.EgoSkill => "Active",
        SkillType.KnowledgeSkill => "Know",
        SkillType.ExoticSkill => "Exotic",
        _ => "Unknown"
    };
}
