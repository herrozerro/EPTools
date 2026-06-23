using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using EPTools.Core.Interfaces;
using EPTools.Core.Models.Ego;

namespace EPTools.Desktop.ViewModels;

public partial class InventoryItemViewModel : ObservableObject
{
    private readonly InventoryItem _model;

    public InventoryItemViewModel(InventoryItem model)
    {
        _model = model;
    }

    public InventoryItem Model => _model;

    public Guid InstanceId => _model.InstanceId;

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

    public int Quantity
    {
        get => _model.Quantity;
        set => SetProperty(_model.Quantity, value, _model, (m, v) => m.Quantity = v);
    }

    public bool Equipped
    {
        get => _model.Equipped;
        set => SetProperty(_model.Equipped, value, _model, (m, v) => m.Equipped = v);
    }

    public bool Active
    {
        get => _model.Active;
        set => SetProperty(_model.Active, value, _model, (m, v) => m.Active = v);
    }

    public string Notes
    {
        get => _model.Notes;
        set => SetProperty(_model.Notes, value, _model, (m, v) => m.Notes = v);
    }

    // Properties from BaseGear (read-only reference data)
    public string Category => _model.BaseGear?.Category ?? string.Empty;
    public string Subcategory => _model.BaseGear?.Subcategory ?? string.Empty;
    public string Complexity => _model.BaseGear?.Complexity ?? string.Empty;
    public string BaseDescription => _model.BaseGear?.Description ?? string.Empty;
    public string Summary => _model.BaseGear?.Summary ?? string.Empty;
}

public partial class InventoryCacheViewModel : ObservableObject
{
    private readonly InventoryCache _model;
    private readonly IEgoManager _egoManager;

    public InventoryCacheViewModel(InventoryCache model, IEgoManager egoManager)
    {
        _model = model;
        _egoManager = egoManager;

        foreach (var item in model.Inventory)
            Items.Add(new InventoryItemViewModel(item));

        foreach (var morph in model.Morphs)
            Morphs.Add(new MorphViewModel(morph, _egoManager));
    }

    public InventoryCache Model => _model;

    public string Location
    {
        get => _model.Location;
        set => SetProperty(_model.Location, value, _model, (m, v) => m.Location = v);
    }

    public ObservableCollection<InventoryItemViewModel> Items { get; } = [];
    public ObservableCollection<MorphViewModel> Morphs { get; } = [];

    public void AddItem(InventoryItem item)
    {
        _egoManager.AddItemToCache(_model, item);
        Items.Add(new InventoryItemViewModel(item));
    }

    public void RemoveItem(InventoryItemViewModel itemVm)
    {
        _egoManager.RemoveItemFromCache(_model, itemVm.Model);
        Items.Remove(itemVm);
    }

    public void AddMorphViewModel(Morph morph)
    {
        Morphs.Add(new MorphViewModel(morph, _egoManager));
    }

    public void RemoveMorphViewModel(MorphViewModel morphVm)
    {
        Morphs.Remove(morphVm);
    }
}
