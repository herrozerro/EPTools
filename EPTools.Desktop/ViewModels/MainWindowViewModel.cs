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

    public ObservableCollection<EgoAptitudeViewModel> Aptitudes { get; } = [];
    public ObservableCollection<IdentityViewModel> Identities { get; } = [];
    public ObservableCollection<EgoSkillViewModel> Skills { get; } = [];

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

        // Notify grouped collections changed
        OnPropertyChanged(nameof(ActiveSkills));
        OnPropertyChanged(nameof(KnowledgeSkills));
        OnPropertyChanged(nameof(ExoticSkills));
    }

    public void AddKnowledgeSkill()
    {
        var skill = new EgoSkill
        {
            Name = "New Knowledge Skill",
            SkillType = SkillType.KnowledgeSkill,
            Aptitude = "Intuition"
        };
        CurrentEgo.Skills.Add(skill);
        Skills.Add(new EgoSkillViewModel(skill, Aptitudes));
        OnPropertyChanged(nameof(KnowledgeSkills));
    }

    public void AddExoticSkill()
    {
        var skill = new EgoSkill
        {
            Name = "New Exotic Skill",
            SkillType = SkillType.ExoticSkill,
            Aptitude = "Reflexes"
        };
        CurrentEgo.Skills.Add(skill);
        Skills.Add(new EgoSkillViewModel(skill, Aptitudes));
        OnPropertyChanged(nameof(ExoticSkills));
    }

    public void DeleteSkill(EgoSkillViewModel skillVm)
    {
        if (skillVm.SkillType == SkillType.EgoSkill) return; // Can't delete active skills

        CurrentEgo.Skills.Remove(skillVm.Model);
        Skills.Remove(skillVm);

        if (skillVm.SkillType == SkillType.KnowledgeSkill)
            OnPropertyChanged(nameof(KnowledgeSkills));
        else
            OnPropertyChanged(nameof(ExoticSkills));
    }

    public MainWindowViewModel(
        EgoService egoService,
        LifepathService lifepathService,
        IUserDataStore userDataStore)
    {
        _egoService = egoService;
        _lifepathService = lifepathService;
        _userDataStore = userDataStore;

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
