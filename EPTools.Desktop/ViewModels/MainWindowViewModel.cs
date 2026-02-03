using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EPTools.Core.Interfaces;
using EPTools.Core.Models.Ego;
using EPTools.Core.Services;

namespace EPTools.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private const string StorageKey = "EP_CurrentCharacter";

    private readonly EgoService _egoService;
    private readonly LifepathService _lifepathService;
    private readonly IUserDataStore _userDataStore;
    private readonly EgoManager _egoManager;

    [ObservableProperty]
    private Ego _currentEgo = new();

    [ObservableProperty]
    private bool _isLoading = true;

    [ObservableProperty]
    private int _selectedTabIndex;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private IdentityViewModel? _selectedIdentity;

    [ObservableProperty]
    private MorphViewModel? _selectedMorph;

    [ObservableProperty]
    private InventoryCacheViewModel? _selectedCache;

    public ObservableCollection<EgoAptitudeViewModel> Aptitudes { get; } = [];
    public ObservableCollection<IdentityViewModel> Identities { get; } = [];
    public ObservableCollection<EgoSkillViewModel> Skills { get; } = [];
    public ObservableCollection<MorphViewModel> Morphs { get; } = [];
    public ObservableCollection<InventoryItemViewModel> Inventory { get; } = [];
    public ObservableCollection<InventoryCacheViewModel> Caches { get; } = [];
    public ObservableCollection<EgoTraitViewModel> EgoTraits { get; } = [];

    // Grouped skill collections for the UI
    public IEnumerable<EgoSkillViewModel> ActiveSkills =>
        Skills.Where(s => s.SkillType == SkillType.EgoSkill).OrderBy(s => s.Name);

    public IEnumerable<EgoSkillViewModel> KnowledgeSkills =>
        Skills.Where(s => s.SkillType == SkillType.KnowledgeSkill).OrderBy(s => s.Name);

    public IEnumerable<EgoSkillViewModel> ExoticSkills =>
        Skills.Where(s => s.SkillType == SkillType.ExoticSkill).OrderBy(s => s.Name);

    partial void OnCurrentEgoChanged(Ego value)
    {
        // Rebuild aptitude view models
        Aptitudes.Clear();
        foreach (var apt in value.Aptitudes)
        {
            Aptitudes.Add(new EgoAptitudeViewModel(apt));
        }

        // Rebuild skill view models
        Skills.Clear();
        foreach (var skill in value.Skills)
        {
            Skills.Add(new EgoSkillViewModel(skill, Aptitudes));
        }

        // Rebuild identity view models
        Identities.Clear();
        foreach (var identity in value.Identities)
        {
            Identities.Add(new IdentityViewModel(identity));
        }

        // Select first identity when ego changes
        SelectedIdentity = Identities.FirstOrDefault();

        // Rebuild morph view models
        Morphs.Clear();
        foreach (var morph in value.Morphs)
        {
            Morphs.Add(new MorphViewModel(morph));
        }

        // Select active morph or first morph
        SelectedMorph = Morphs.FirstOrDefault(m => m.ActiveMorph) ?? Morphs.FirstOrDefault();

        // Rebuild inventory view models
        Inventory.Clear();
        foreach (var item in value.Inventory)
        {
            Inventory.Add(new InventoryItemViewModel(item));
        }

        // Rebuild cache view models
        Caches.Clear();
        foreach (var cache in value.Caches)
        {
            Caches.Add(new InventoryCacheViewModel(cache));
        }
        SelectedCache = Caches.FirstOrDefault();

        // Rebuild ego trait view models
        EgoTraits.Clear();
        foreach (var trait in value.EgoTraits)
        {
            EgoTraits.Add(new EgoTraitViewModel(trait));
        }

        // Notify grouped collections changed
        OnPropertyChanged(nameof(ActiveSkills));
        OnPropertyChanged(nameof(KnowledgeSkills));
        OnPropertyChanged(nameof(ExoticSkills));
    }

    public void AddKnowledgeSkill()
    {
        var skill = _egoManager.AddKnowledgeSkill(CurrentEgo);
        Skills.Add(new EgoSkillViewModel(skill, Aptitudes));
        OnPropertyChanged(nameof(KnowledgeSkills));
    }

    public void AddExoticSkill()
    {
        var skill = _egoManager.AddExoticSkill(CurrentEgo);
        Skills.Add(new EgoSkillViewModel(skill, Aptitudes));
        OnPropertyChanged(nameof(ExoticSkills));
    }

    public void DeleteSkill(EgoSkillViewModel skillVm)
    {
        if (!_egoManager.RemoveSkill(CurrentEgo, skillVm.Model))
            return;

        Skills.Remove(skillVm);

        if (skillVm.SkillType == SkillType.KnowledgeSkill)
            OnPropertyChanged(nameof(KnowledgeSkills));
        else
            OnPropertyChanged(nameof(ExoticSkills));
    }

    public void AddMorph()
    {
        var morph = _egoManager.AddMorph(CurrentEgo);
        var vm = new MorphViewModel(morph);
        Morphs.Add(vm);
        SelectedMorph = vm;
    }

    public void DeleteMorph(MorphViewModel morphVm)
    {
        if (!_egoManager.RemoveMorph(CurrentEgo, morphVm.Model))
            return;

        Morphs.Remove(morphVm);
        SelectedMorph = Morphs.FirstOrDefault(m => m.ActiveMorph) ?? Morphs.FirstOrDefault();
    }

    public void SetActiveMorph(MorphViewModel morphVm)
    {
        _egoManager.SetActiveMorph(CurrentEgo, morphVm.Model);
        // Update all ViewModel active states to match
        foreach (var m in Morphs)
        {
            m.ActiveMorph = m.Model.ActiveMorph;
        }
    }

    public void AddInventoryItem()
    {
        var item = _egoManager.AddInventoryItem(CurrentEgo);
        Inventory.Add(new InventoryItemViewModel(item));
    }

    public void DeleteInventoryItem(InventoryItemViewModel itemVm)
    {
        if (_egoManager.RemoveInventoryItem(CurrentEgo, itemVm.Model))
        {
            Inventory.Remove(itemVm);
        }
    }

    public void AddCache()
    {
        var cache = _egoManager.AddCache(CurrentEgo);
        var vm = new InventoryCacheViewModel(cache);
        Caches.Add(vm);
        SelectedCache = vm;
    }

    public void DeleteCache(InventoryCacheViewModel cacheVm)
    {
        if (_egoManager.RemoveCache(CurrentEgo, cacheVm.Model))
        {
            Caches.Remove(cacheVm);
            SelectedCache = Caches.FirstOrDefault();
        }
    }

    public void AddItemToCache(InventoryCacheViewModel cacheVm)
    {
        var item = _egoManager.AddItemToCache(cacheVm.Model);
        cacheVm.Items.Add(new InventoryItemViewModel(item));
    }

    public void DeleteItemFromCache(InventoryCacheViewModel cacheVm, InventoryItemViewModel itemVm)
    {
        if (_egoManager.RemoveItemFromCache(cacheVm.Model, itemVm.Model))
        {
            cacheVm.Items.Remove(itemVm);
        }
    }

    public void MoveItemToCache(InventoryItemViewModel itemVm, InventoryCacheViewModel cacheVm)
    {
        if (_egoManager.MoveItemToCache(CurrentEgo, itemVm.Model, cacheVm.Model))
        {
            Inventory.Remove(itemVm);
            cacheVm.Items.Add(itemVm);
        }
    }

    public void MoveItemFromCache(InventoryItemViewModel itemVm, InventoryCacheViewModel cacheVm)
    {
        if (_egoManager.MoveItemFromCache(CurrentEgo, itemVm.Model, cacheVm.Model))
        {
            cacheVm.Items.Remove(itemVm);
            Inventory.Add(itemVm);
        }
    }

    public void AddEgoTrait()
    {
        var trait = _egoManager.AddEgoTrait(CurrentEgo);
        EgoTraits.Add(new EgoTraitViewModel(trait));
    }

    public void DeleteEgoTrait(EgoTraitViewModel traitVm)
    {
        if (_egoManager.RemoveEgoTrait(CurrentEgo, traitVm.Model))
        {
            EgoTraits.Remove(traitVm);
        }
    }

    public MainWindowViewModel(
        EgoService egoService,
        LifepathService lifepathService,
        IUserDataStore userDataStore,
        EgoManager egoManager)
    {
        _egoService = egoService;
        _lifepathService = lifepathService;
        _userDataStore = userDataStore;
        _egoManager = egoManager;

        // Load character on startup
        _ = LoadCharacterAsync();
    }

    private async Task LoadCharacterAsync()
    {
        IsLoading = true;
        try
        {
            var storedEgo = await _userDataStore.GetItemAsync<Ego>(StorageKey);
            if (storedEgo != null)
            {
                CurrentEgo = storedEgo;
                StatusMessage = "Character loaded from storage.";
            }
            else
            {
                CurrentEgo = await _egoService.GetDefaults();
                StatusMessage = "New character created.";
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error loading character: {ex.Message}";
            CurrentEgo = await _egoService.GetDefaults();
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task GenerateLifepathAsync()
    {
        IsLoading = true;
        StatusMessage = "Generating character...";
        try
        {
            CurrentEgo = await _lifepathService.GenerateEgo();
            StatusMessage = "Character generated!";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error generating character: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        try
        {
            await _userDataStore.SaveItemAsync(StorageKey, CurrentEgo);
            StatusMessage = "Character saved!";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error saving character: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task ResetAsync()
    {
        try
        {
            await _userDataStore.DeleteItemAsync(StorageKey);
            CurrentEgo = await _egoService.GetDefaults();
            StatusMessage = "Character reset.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error resetting character: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task ExportAsync(IStorageProvider? storageProvider)
    {
        if (storageProvider == null) return;

        try
        {
            var file = await storageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Export Character",
                SuggestedFileName = $"{CurrentEgo.Name}.json",
                FileTypeChoices = new[]
                {
                    new FilePickerFileType("JSON") { Patterns = new[] { "*.json" } }
                }
            });

            if (file != null)
            {
                await using var stream = await file.OpenWriteAsync();
                await JsonSerializer.SerializeAsync(stream, CurrentEgo, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                StatusMessage = "Character exported!";
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error exporting: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task ImportAsync(IStorageProvider? storageProvider)
    {
        if (storageProvider == null) return;

        try
        {
            var files = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Import Character",
                AllowMultiple = false,
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("JSON") { Patterns = new[] { "*.json" } }
                }
            });

            if (files.Count > 0)
            {
                await using var stream = await files[0].OpenReadAsync();
                var imported = await JsonSerializer.DeserializeAsync<Ego>(stream);
                if (imported != null)
                {
                    CurrentEgo = imported;
                    await _userDataStore.SaveItemAsync(StorageKey, CurrentEgo);
                    StatusMessage = "Character imported!";
                }
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error importing: {ex.Message}";
        }
    }
}
