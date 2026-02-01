using System;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EPTools.Core.Interfaces;
using EPTools.Core.Models.Ego;
using EPTools.Core.Services;

namespace EpTools.Desktop.ViewModels;

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
