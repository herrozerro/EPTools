using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using EPTools.Core.Interfaces;
using EPTools.Core.Models.Ego;

namespace EPTools.Desktop.ViewModels;

public partial class MorphViewModel : ObservableObject
{
    private readonly Morph _model;
    private readonly IEgoManager _egoManager;

    public static IReadOnlyList<string> AvailableMorphTypes { get; } = new[]
    {
        "Biomorph", "Pod", "Synthmorph", "Infomorph", "Uplift", "Exomorph"
    };

    public static IReadOnlyList<string> AvailableSizes { get; } = new[]
    {
        "Very Small", "Small", "Medium", "Large", "Very Large"
    };

    public MorphViewModel(Morph model, IEgoManager egoManager)
    {
        _model = model;
        _egoManager = egoManager;

        foreach (var trait in model.Traits)
            Traits.Add(new MorphTraitViewModel(trait));

        foreach (var ware in model.Wares)
            Wares.Add(new MorphWareViewModel(ware));
    }

    public Morph Model => _model;

    public string Name
    {
        get => _model.Name;
        set => SetProperty(_model.Name, value, _model, (m, v) => m.Name = v);
    }

    public string MorphType
    {
        get => _model.MorphType;
        set => SetProperty(_model.MorphType, value, _model, (m, v) => m.MorphType = v);
    }

    public string MorphSex
    {
        get => _model.MorphSex;
        set => SetProperty(_model.MorphSex, value, _model, (m, v) => m.MorphSex = v);
    }

    public string Size
    {
        get => _model.Size;
        set => SetProperty(_model.Size, value, _model, (m, v) => m.Size = v);
    }

    public int Vigor
    {
        get => _model.Vigor;
        set => SetProperty(_model.Vigor, value, _model, (m, v) => m.Vigor = v);
    }

    public int Insight
    {
        get => _model.Insight;
        set => SetProperty(_model.Insight, value, _model, (m, v) => m.Insight = v);
    }

    public int Moxie
    {
        get => _model.Moxie;
        set => SetProperty(_model.Moxie, value, _model, (m, v) => m.Moxie = v);
    }

    public int MorphFlex
    {
        get => _model.MorphFlex;
        set => SetProperty(_model.MorphFlex, value, _model, (m, v) => m.MorphFlex = v);
    }

    public bool ActiveMorph
    {
        get => _model.ActiveMorph;
        set => SetProperty(_model.ActiveMorph, value, _model, (m, v) => m.ActiveMorph = v);
    }

    public ObservableCollection<MorphTraitViewModel> Traits { get; } = [];
    public ObservableCollection<MorphWareViewModel> Wares { get; } = [];

    public void AddTrait()
    {
        var trait = _egoManager.AddMorphTrait(_model);
        Traits.Add(new MorphTraitViewModel(trait));
    }

    public void RemoveTrait(MorphTraitViewModel traitVm)
    {
        _egoManager.RemoveMorphTrait(_model, traitVm.Model);
        Traits.Remove(traitVm);
    }

    public void AddWare()
    {
        var ware = _egoManager.AddMorphWare(_model);
        Wares.Add(new MorphWareViewModel(ware));
    }

    public void RemoveWare(MorphWareViewModel wareVm)
    {
        _egoManager.RemoveMorphWare(_model, wareVm.Model);
        Wares.Remove(wareVm);
    }
}

public partial class MorphTraitViewModel : ObservableObject
{
    private readonly EgoTrait _model;

    public MorphTraitViewModel(EgoTrait model)
    {
        _model = model;
    }

    public EgoTrait Model => _model;

    public string Name
    {
        get => _model.Name;
        set => SetProperty(_model.Name, value, _model, (m, v) => m.Name = v);
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

    public string Summary
    {
        get => _model.Summary;
        set => SetProperty(_model.Summary, value, _model, (m, v) => m.Summary = v);
    }
}

public partial class MorphWareViewModel : ObservableObject
{
    private readonly Ware _model;

    public MorphWareViewModel(Ware model)
    {
        _model = model;
    }

    public Ware Model => _model;

    public string Name
    {
        get => _model.Name;
        set => SetProperty(_model.Name, value, _model, (m, v) => m.Name = v);
    }

    public string Description
    {
        get => _model.Description;
        set => SetProperty(_model.Description, value, _model, (m, v) => m.Description = v);
    }

    public bool Active
    {
        get => _model.Active;
        set => SetProperty(_model.Active, value, _model, (m, v) => m.Active = v);
    }

    public int Cost
    {
        get => _model.Cost;
        set => SetProperty(_model.Cost, value, _model, (m, v) => m.Cost = v);
    }

    public int Quantity
    {
        get => _model.Quantity;
        set => SetProperty(_model.Quantity, value, _model, (m, v) => m.Quantity = v);
    }
}
