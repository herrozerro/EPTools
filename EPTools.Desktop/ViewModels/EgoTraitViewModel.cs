using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using EPTools.Core.Models.Ego;

namespace EPTools.Desktop.ViewModels;

public partial class EgoTraitViewModel : ObservableObject
{
    private readonly EgoTrait _model;

    public static IReadOnlyList<string> AvailableTypes { get; } = new[]
    {
        "Positive", "Negative", "Neutral"
    };

    public EgoTraitViewModel(EgoTrait model)
    {
        _model = model;
    }

    public EgoTrait Model => _model;

    public string Name
    {
        get => _model.Name;
        set => SetProperty(_model.Name, value, _model, (m, v) => m.Name = v);
    }

    public string Type
    {
        get => _model.Type;
        set => SetProperty(_model.Type, value, _model, (m, v) => m.Type = v);
    }

    public int Level
    {
        get => _model.Level;
        set => SetProperty(_model.Level, value, _model, (m, v) => m.Level = v);
    }

    public int Cost
    {
        get => _model.Cost;
        set => SetProperty(_model.Cost, value, _model, (m, v) => m.Cost = v);
    }

    public string CostTiers
    {
        get => _model.CostTiers;
        set => SetProperty(_model.CostTiers, value, _model, (m, v) => m.CostTiers = v);
    }

    public string Summary
    {
        get => _model.Summary;
        set => SetProperty(_model.Summary, value, _model, (m, v) => m.Summary = v);
    }

    public string Description
    {
        get => _model.Description;
        set => SetProperty(_model.Description, value, _model, (m, v) => m.Description = v);
    }

    public string TypeDisplay => Type switch
    {
        "Positive" => "+",
        "Negative" => "-",
        _ => "~"
    };
}
