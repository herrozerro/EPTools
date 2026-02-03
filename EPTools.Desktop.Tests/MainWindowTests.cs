using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Headless.XUnit;
using Avalonia.VisualTree;
using EPTools.Desktop.ViewModels;
using EPTools.Desktop.Views;
using EPTools.Core.Interfaces;
using EPTools.Core.Models.Ego;
using EPTools.Core.Services;

namespace EPTools.Desktop.Tests;

#region Mock Services

public class MockUserDataStore : IUserDataStore
{
    private readonly Dictionary<string, object?> _storage = new();

    public Task<T?> GetItemAsync<T>(string key)
    {
        if (_storage.TryGetValue(key, out var value) && value is T typedValue)
            return Task.FromResult<T?>(typedValue);
        return Task.FromResult<T?>(default);
    }

    public Task SaveItemAsync<T>(string key, T item)
    {
        _storage[key] = item;
        return Task.CompletedTask;
    }

    public Task DeleteItemAsync(string key)
    {
        _storage.Remove(key);
        return Task.CompletedTask;
    }
}

public class MockFetchService : IFetchService
{
    public Task<T> GetTFromFileAsync<T>(string filename) where T : new()
    {
        return Task.FromResult(new T());
    }

    public Task<T> GetTFromEpFileAsync<T>(string filename) where T : new()
    {
        // Return mock data based on filename
        if (typeof(T) == typeof(List<EPTools.Core.Models.EPDataModels.Aptitude>))
        {
            var aptitudes = new List<EPTools.Core.Models.EPDataModels.Aptitude>
            {
                new("Cognition", "Intelligence", "COG", "EP2", "", []),
                new("Intuition", "Gut instinct", "INT", "EP2", "", []),
                new("Reflexes", "Coordination", "REF", "EP2", "", []),
                new("Savvy", "Social awareness", "SAV", "EP2", "", []),
                new("Somatics", "Physical capability", "SOM", "EP2", "", []),
                new("Willpower", "Mental fortitude", "WIL", "EP2", "", [])
            };
            return Task.FromResult((T)(object)aptitudes);
        }

        if (typeof(T) == typeof(List<EPTools.Core.Models.EPDataModels.Skill>))
        {
            var skills = new List<EPTools.Core.Models.EPDataModels.Skill>();
            return Task.FromResult((T)(object)skills);
        }

        return Task.FromResult(new T());
    }
}

#endregion

public class MainWindowViewModelTests
{
    private MainWindowViewModel CreateViewModel()
    {
        var fetchService = new MockFetchService();
        var userDataStore = new MockUserDataStore();
        var dataService = new EpDataService(fetchService, userDataStore);
        var egoService = new EgoService(dataService);
        var lifepathService = new LifepathService(dataService, egoService);
        var egoManager = new EgoManager();

        return new MainWindowViewModel(egoService, lifepathService, userDataStore, egoManager);
    }

    #region ViewModel Creation Tests

    [AvaloniaFact]
    public void ViewModel_Can_Be_Created()
    {
        var viewModel = CreateViewModel();
        Assert.NotNull(viewModel);
    }

    [AvaloniaFact]
    public async Task ViewModel_Loads_Default_Ego_On_Creation()
    {
        var viewModel = CreateViewModel();

        // Wait for async loading to complete
        await Task.Delay(100);

        Assert.NotNull(viewModel.CurrentEgo);
    }

    [AvaloniaFact]
    public async Task ViewModel_Populates_Aptitudes_After_Loading()
    {
        var viewModel = CreateViewModel();

        // Wait for async loading
        await Task.Delay(100);

        // Should have 6 aptitudes (COG, INT, REF, SAV, SOM, WIL)
        Assert.Equal(6, viewModel.Aptitudes.Count);
    }

    [AvaloniaFact]
    public async Task ViewModel_Populates_Identities_After_Loading()
    {
        var viewModel = CreateViewModel();

        // Wait for async loading
        await Task.Delay(100);

        // Should have at least 1 default identity
        Assert.NotEmpty(viewModel.Identities);
    }

    [AvaloniaFact]
    public async Task ViewModel_Sets_SelectedIdentity_After_Loading()
    {
        var viewModel = CreateViewModel();

        // Wait for async loading
        await Task.Delay(100);

        Assert.NotNull(viewModel.SelectedIdentity);
    }

    #endregion

    #region Property Tests

    [AvaloniaFact]
    public async Task ViewModel_IsLoading_Becomes_False_After_Loading()
    {
        var viewModel = CreateViewModel();

        // Wait for loading to complete
        await Task.Delay(200);

        Assert.False(viewModel.IsLoading);
    }

    [AvaloniaFact]
    public async Task ViewModel_IsLoading_False_After_Loading()
    {
        var viewModel = CreateViewModel();

        // Wait for async loading
        await Task.Delay(100);

        Assert.False(viewModel.IsLoading);
    }

    [AvaloniaFact]
    public async Task ViewModel_StatusMessage_Set_After_Loading()
    {
        var viewModel = CreateViewModel();

        // Wait for async loading
        await Task.Delay(100);

        Assert.False(string.IsNullOrEmpty(viewModel.StatusMessage));
    }

    [AvaloniaFact]
    public void ViewModel_SelectedTabIndex_Defaults_To_Zero()
    {
        var viewModel = CreateViewModel();
        Assert.Equal(0, viewModel.SelectedTabIndex);
    }

    [AvaloniaFact]
    public void ViewModel_SelectedTabIndex_Can_Be_Changed()
    {
        var viewModel = CreateViewModel();

        viewModel.SelectedTabIndex = 2;

        Assert.Equal(2, viewModel.SelectedTabIndex);
    }

    #endregion

    #region CurrentEgo Property Tests

    [AvaloniaFact]
    public async Task CurrentEgo_Name_Can_Be_Set()
    {
        var viewModel = CreateViewModel();
        await Task.Delay(100);

        viewModel.CurrentEgo.Name = "Test Character";

        Assert.Equal("Test Character", viewModel.CurrentEgo.Name);
    }

    [AvaloniaFact]
    public async Task CurrentEgo_Background_Can_Be_Set()
    {
        var viewModel = CreateViewModel();
        await Task.Delay(100);

        viewModel.CurrentEgo.Background = "Colonist";

        Assert.Equal("Colonist", viewModel.CurrentEgo.Background);
    }

    [AvaloniaFact]
    public async Task CurrentEgo_Career_Can_Be_Set()
    {
        var viewModel = CreateViewModel();
        await Task.Delay(100);

        viewModel.CurrentEgo.Career = "Hacker";

        Assert.Equal("Hacker", viewModel.CurrentEgo.Career);
    }

    #endregion

    #region Aptitude ViewModel Tests

    [AvaloniaFact]
    public async Task Aptitude_ViewModels_Wrap_Model_Correctly()
    {
        var viewModel = CreateViewModel();
        await Task.Delay(100);

        var cogAptitude = viewModel.Aptitudes.FirstOrDefault(a => a.ShortName == "COG");

        Assert.NotNull(cogAptitude);
        Assert.Equal("Cognition", cogAptitude.Name);
    }

    [AvaloniaFact]
    public async Task Aptitude_Value_Change_Updates_CheckRating()
    {
        var viewModel = CreateViewModel();
        await Task.Delay(100);

        var aptitude = viewModel.Aptitudes.First();
        aptitude.AptitudeValue = 10;

        // CheckRating = AptitudeValue * 3 + CheckMod
        Assert.Equal(30, aptitude.CheckRating);
    }

    [AvaloniaFact]
    public async Task Aptitude_Value_Change_Persists_To_Ego()
    {
        var viewModel = CreateViewModel();
        await Task.Delay(100);

        var aptitudeVm = viewModel.Aptitudes.First();
        aptitudeVm.AptitudeValue = 15;

        var egoAptitude = viewModel.CurrentEgo.Aptitudes.First();
        Assert.Equal(15, egoAptitude.AptitudeValue);
    }

    #endregion

    #region Command Tests

    [AvaloniaFact]
    public async Task SaveCommand_Exists()
    {
        var viewModel = CreateViewModel();
        await Task.Delay(100);

        Assert.NotNull(viewModel.SaveCommand);
    }

    [AvaloniaFact]
    public async Task ResetCommand_Exists()
    {
        var viewModel = CreateViewModel();
        await Task.Delay(100);

        Assert.NotNull(viewModel.ResetCommand);
    }

    [AvaloniaFact]
    public async Task GenerateLifepathCommand_Exists()
    {
        var viewModel = CreateViewModel();
        await Task.Delay(100);

        Assert.NotNull(viewModel.GenerateLifepathCommand);
    }

    [AvaloniaFact]
    public async Task SaveCommand_Updates_StatusMessage()
    {
        var viewModel = CreateViewModel();
        await Task.Delay(100);

        await viewModel.SaveCommand.ExecuteAsync(null);

        Assert.Contains("saved", viewModel.StatusMessage.ToLower());
    }

    #endregion
}

public class MainWindowUITests
{
    private (Window window, MainWindowViewModel viewModel) CreateWindowWithViewModel()
    {
        var fetchService = new MockFetchService();
        var userDataStore = new MockUserDataStore();
        var dataService = new EpDataService(fetchService, userDataStore);
        var egoService = new EgoService(dataService);
        var lifepathService = new LifepathService(dataService, egoService);
        var egoManager = new EgoManager();

        var viewModel = new MainWindowViewModel(egoService, lifepathService, userDataStore, egoManager);

        var window = new MainWindow
        {
            DataContext = viewModel
        };

        window.Show();
        window.UpdateLayout();

        return (window, viewModel);
    }

    #region Window Creation Tests

    [AvaloniaFact]
    public void MainWindow_Can_Be_Created()
    {
        var window = new MainWindow();
        Assert.NotNull(window);
    }

    [AvaloniaFact]
    public void MainWindow_Has_Correct_Title()
    {
        var window = new MainWindow();
        Assert.Equal("EP Tools - Character Sheet", window.Title);
    }

    [AvaloniaFact]
    public void MainWindow_Has_Correct_Default_Size()
    {
        var window = new MainWindow();
        Assert.Equal(1200, window.Width);
        Assert.Equal(800, window.Height);
    }

    #endregion

    #region Toolbar Tests

    [AvaloniaFact]
    public async Task Window_Has_Toolbar_Buttons()
    {
        var (window, viewModel) = CreateWindowWithViewModel();
        await Task.Delay(100);

        try
        {
            var buttons = window.GetVisualDescendants().OfType<Button>().ToList();

            // Should have at least 5 buttons: Generate, Save, Reset, Export, Import
            Assert.True(buttons.Count >= 5);

            var buttonContents = buttons.Select(b => b.Content?.ToString()).ToList();
            Assert.Contains("Generate Lifepath", buttonContents);
            Assert.Contains("Save", buttonContents);
            Assert.Contains("Reset", buttonContents);
            Assert.Contains("Export", buttonContents);
            Assert.Contains("Import", buttonContents);
        }
        finally
        {
            window.Close();
        }
    }

    #endregion

    #region Bio Info Section Tests

    [AvaloniaFact]
    public async Task Window_Has_Name_TextBox()
    {
        var (window, viewModel) = CreateWindowWithViewModel();
        await Task.Delay(100);

        try
        {
            var textBoxes = window.GetVisualDescendants().OfType<TextBox>().ToList();
            var nameTextBox = textBoxes.FirstOrDefault(tb => tb.Watermark == "Name");

            Assert.NotNull(nameTextBox);
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaFact]
    public async Task Window_Has_Background_TextBox()
    {
        var (window, viewModel) = CreateWindowWithViewModel();
        await Task.Delay(100);

        try
        {
            var textBoxes = window.GetVisualDescendants().OfType<TextBox>().ToList();
            var backgroundTextBox = textBoxes.FirstOrDefault(tb => tb.Watermark == "Background");

            Assert.NotNull(backgroundTextBox);
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaFact]
    public async Task Window_Has_Career_TextBox()
    {
        var (window, viewModel) = CreateWindowWithViewModel();
        await Task.Delay(100);

        try
        {
            var textBoxes = window.GetVisualDescendants().OfType<TextBox>().ToList();
            var careerTextBox = textBoxes.FirstOrDefault(tb => tb.Watermark == "Career");

            Assert.NotNull(careerTextBox);
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaFact]
    public async Task Name_TextBox_Can_Update_ViewModel()
    {
        var (window, viewModel) = CreateWindowWithViewModel();
        await Task.Delay(200);

        try
        {
            var textBoxes = window.GetVisualDescendants().OfType<TextBox>().ToList();
            var nameTextBox = textBoxes.FirstOrDefault(tb => tb.Watermark == "Name");

            Assert.NotNull(nameTextBox);

            // Update the TextBox directly
            nameTextBox.Text = "Test Name";
            window.UpdateLayout();

            // Verify the ViewModel was updated
            Assert.Equal("Test Name", viewModel.CurrentEgo.Name);
        }
        finally
        {
            window.Close();
        }
    }

    #endregion

    #region Aptitudes Section Tests

    [AvaloniaFact]
    public async Task Window_Has_Aptitudes_Section()
    {
        var (window, viewModel) = CreateWindowWithViewModel();
        await Task.Delay(100);

        try
        {
            var textBlocks = window.GetVisualDescendants().OfType<TextBlock>().ToList();
            var aptitudesHeader = textBlocks.FirstOrDefault(tb => tb.Text == "Aptitudes");

            Assert.NotNull(aptitudesHeader);
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaFact]
    public async Task Window_Shows_All_Six_Aptitudes()
    {
        var (window, viewModel) = CreateWindowWithViewModel();
        await Task.Delay(100);

        try
        {
            var textBlocks = window.GetVisualDescendants().OfType<TextBlock>().ToList();
            var aptitudeNames = new[] { "COG", "INT", "REF", "SAV", "SOM", "WIL" };

            foreach (var name in aptitudeNames)
            {
                Assert.Contains(textBlocks, tb => tb.Text == name);
            }
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaFact]
    public async Task Aptitude_NumericUpDown_Updates_ViewModel()
    {
        var (window, viewModel) = CreateWindowWithViewModel();
        await Task.Delay(100);

        try
        {
            // Find the ItemsControl containing aptitudes
            var itemsControl = window.GetVisualDescendants().OfType<ItemsControl>().FirstOrDefault();
            Assert.NotNull(itemsControl);

            // Find NumericUpDowns inside
            var numericUpDowns = window.GetVisualDescendants().OfType<NumericUpDown>().ToList();

            // Find one in the aptitudes section (they should be width 70)
            var aptitudeNud = numericUpDowns.FirstOrDefault(n => n.Width == 70);

            if (aptitudeNud != null)
            {
                aptitudeNud.Value = 20;

                // Check that a ViewModel aptitude was updated
                var updatedAptitude = viewModel.Aptitudes.FirstOrDefault(a => a.AptitudeValue == 20);
                Assert.NotNull(updatedAptitude);
            }
        }
        finally
        {
            window.Close();
        }
    }

    #endregion

    #region Tab Control Tests

    [AvaloniaFact]
    public async Task Window_Has_TabControl()
    {
        var (window, viewModel) = CreateWindowWithViewModel();
        await Task.Delay(100);

        try
        {
            var tabControl = window.GetVisualDescendants().OfType<TabControl>().FirstOrDefault();
            Assert.NotNull(tabControl);
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaFact]
    public async Task TabControl_Has_Five_Tabs()
    {
        var (window, viewModel) = CreateWindowWithViewModel();
        await Task.Delay(100);

        try
        {
            var tabControl = window.GetVisualDescendants().OfType<TabControl>().FirstOrDefault();
            Assert.NotNull(tabControl);
            Assert.Equal(5, tabControl.Items.Count);
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaFact]
    public async Task TabControl_Has_Correct_Tab_Headers()
    {
        var (window, viewModel) = CreateWindowWithViewModel();
        await Task.Delay(100);

        try
        {
            var tabItems = window.GetVisualDescendants().OfType<TabItem>().ToList();
            var headers = tabItems.Select(t => t.Header?.ToString()).ToList();

            Assert.Contains("Skills", headers);
            Assert.Contains("Network", headers);
            Assert.Contains("Morph", headers);
            Assert.Contains("Gear", headers);
            Assert.Contains("Traits", headers);
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaFact]
    public async Task TabControl_SelectedIndex_Binds_To_ViewModel()
    {
        var (window, viewModel) = CreateWindowWithViewModel();
        await Task.Delay(100);

        try
        {
            var tabControl = window.GetVisualDescendants().OfType<TabControl>().FirstOrDefault();
            Assert.NotNull(tabControl);

            viewModel.SelectedTabIndex = 2;
            window.UpdateLayout();

            Assert.Equal(2, tabControl.SelectedIndex);
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaFact]
    public async Task Changing_Tab_Updates_ViewModel()
    {
        var (window, viewModel) = CreateWindowWithViewModel();
        await Task.Delay(100);

        try
        {
            var tabControl = window.GetVisualDescendants().OfType<TabControl>().FirstOrDefault();
            Assert.NotNull(tabControl);

            tabControl.SelectedIndex = 3;
            window.UpdateLayout();

            Assert.Equal(3, viewModel.SelectedTabIndex);
        }
        finally
        {
            window.Close();
        }
    }

    #endregion

    #region Status Bar Tests

    [AvaloniaFact]
    public async Task Window_Has_StatusMessage_TextBlock()
    {
        var (window, viewModel) = CreateWindowWithViewModel();
        await Task.Delay(100);

        try
        {
            // Status message should be set after loading
            var textBlocks = window.GetVisualDescendants().OfType<TextBlock>().ToList();
            var statusBlock = textBlocks.FirstOrDefault(tb =>
                tb.Text?.Contains("Character") == true ||
                tb.Text?.Contains("loaded") == true ||
                tb.Text?.Contains("created") == true);

            Assert.NotNull(statusBlock);
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaFact]
    public async Task Window_Has_ProgressBar()
    {
        var (window, viewModel) = CreateWindowWithViewModel();
        await Task.Delay(100);

        try
        {
            var progressBar = window.GetVisualDescendants().OfType<ProgressBar>().FirstOrDefault();
            Assert.NotNull(progressBar);
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaFact]
    public async Task ProgressBar_Hidden_When_Not_Loading()
    {
        var (window, viewModel) = CreateWindowWithViewModel();
        await Task.Delay(100);

        try
        {
            var progressBar = window.GetVisualDescendants().OfType<ProgressBar>().FirstOrDefault();
            Assert.NotNull(progressBar);

            // After loading, IsLoading should be false and progress bar should be hidden
            Assert.False(viewModel.IsLoading);
            Assert.False(progressBar.IsVisible);
        }
        finally
        {
            window.Close();
        }
    }

    #endregion
}

public class EgoAptitudeViewModelTests
{
    [AvaloniaFact]
    public void EgoAptitudeViewModel_Wraps_Model()
    {
        var model = new EgoAptitude
        {
            Name = "Cognition",
            ShortName = "COG",
            AptitudeValue = 15,
            CheckMod = 5
        };

        var viewModel = new EgoAptitudeViewModel(model);

        Assert.Equal("Cognition", viewModel.Name);
        Assert.Equal("COG", viewModel.ShortName);
        Assert.Equal(15, viewModel.AptitudeValue);
        Assert.Equal(5, viewModel.CheckMod);
    }

    [AvaloniaFact]
    public void EgoAptitudeViewModel_CheckRating_Calculated_Correctly()
    {
        var model = new EgoAptitude
        {
            AptitudeValue = 10,
            CheckMod = 5
        };

        var viewModel = new EgoAptitudeViewModel(model);

        // CheckRating = AptitudeValue * 3 + CheckMod = 10 * 3 + 5 = 35
        Assert.Equal(35, viewModel.CheckRating);
    }

    [AvaloniaFact]
    public void EgoAptitudeViewModel_AptitudeValue_Change_Updates_Model()
    {
        var model = new EgoAptitude { AptitudeValue = 10 };
        var viewModel = new EgoAptitudeViewModel(model);

        viewModel.AptitudeValue = 20;

        Assert.Equal(20, model.AptitudeValue);
    }

    [AvaloniaFact]
    public void EgoAptitudeViewModel_AptitudeValue_Change_Updates_CheckRating()
    {
        var model = new EgoAptitude { AptitudeValue = 10, CheckMod = 0 };
        var viewModel = new EgoAptitudeViewModel(model);

        Assert.Equal(30, viewModel.CheckRating); // 10 * 3

        viewModel.AptitudeValue = 15;

        Assert.Equal(45, viewModel.CheckRating); // 15 * 3
    }

    [AvaloniaFact]
    public void EgoAptitudeViewModel_CheckMod_Change_Updates_CheckRating()
    {
        var model = new EgoAptitude { AptitudeValue = 10, CheckMod = 0 };
        var viewModel = new EgoAptitudeViewModel(model);

        Assert.Equal(30, viewModel.CheckRating);

        viewModel.CheckMod = 10;

        Assert.Equal(40, viewModel.CheckRating); // 10 * 3 + 10
    }
}
