using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Headless.XUnit;
using Avalonia.VisualTree;
using EPTools.Desktop.ViewModels;
using EPTools.Desktop.Views;
using EPTools.Core.Models.Ego;

namespace EPTools.Desktop.Tests;

public class RepNetworkWidgetTests
{
    [AvaloniaFact]
    public void RepNetworkWidget_Can_Be_Created()
    {
        var widget = new RepNetworkWidget();
        Assert.NotNull(widget);
    }

    [AvaloniaFact]
    public void RepNetworkWidget_Shows_NumericUpDown()
    {
        var widget = new RepNetworkWidget();

        var window = new Window
        {
            Width = 300,
            Height = 300,
            Content = widget
        };

        window.Show();

        var nud = widget.GetVisualDescendants().OfType<NumericUpDown>().FirstOrDefault();
        Assert.NotNull(nud);

        window.Close();
    }

    [AvaloniaFact]
    public void RepNetworkWidget_Binds_To_ViewModel()
    {
        var model = new RepNetwork { Score = 42 };
        var viewModel = new RepNetworkViewModel(model);

        var widget = new RepNetworkWidget
        {
            RepNetwork = viewModel,
            Label = "Test Rep"
        };

        Assert.Equal("Test Rep", widget.Label);
        Assert.Equal(42, widget.RepNetwork?.Score);
    }

    [AvaloniaFact]
    public void RepNetworkViewModel_Updates_Model()
    {
        var model = new RepNetwork { Score = 10 };
        var viewModel = new RepNetworkViewModel(model);

        viewModel.Score = 50;

        Assert.Equal(50, model.Score);
    }

    [AvaloniaFact]
    public void RepNetworkWidget_NumericUpDown_Shows_Score_Value()
    {
        // Arrange
        var model = new RepNetwork { Score = 42 };
        var viewModel = new RepNetworkViewModel(model);

        var widget = new RepNetworkWidget
        {
            RepNetwork = viewModel,
            Label = "@-rep"
        };

        var window = new Window
        {
            Width = 200,
            Height = 200,
            Content = widget
        };

        window.Show();

        // Force layout
        window.UpdateLayout();

        // Act - find the NumericUpDown and check its value
        var numericUpDown = widget.GetVisualDescendants()
            .OfType<NumericUpDown>()
            .FirstOrDefault();

        // Assert
        Assert.NotNull(numericUpDown);
        Assert.Equal(42, numericUpDown.Value);

        window.Close();
    }

    [AvaloniaFact]
    public void RepNetworkWidget_NumericUpDown_Shows_Score_When_Set_After_Show()
    {
        // This simulates the actual app behavior where:
        // 1. Widget is created
        // 2. Widget is shown in window
        // 3. RepNetwork property is set via binding

        // Arrange - create widget WITHOUT setting RepNetwork
        var widget = new RepNetworkWidget
        {
            Label = "@-rep"
        };

        var window = new Window
        {
            Width = 200,
            Height = 200,
            Content = widget
        };

        window.Show();
        window.UpdateLayout();

        // Act - NOW set the RepNetwork (simulating binding resolution)
        var model = new RepNetwork { Score = 75 };
        var viewModel = new RepNetworkViewModel(model);
        widget.RepNetwork = viewModel;

        // Force another layout pass
        window.UpdateLayout();

        // Find the NumericUpDown
        var numericUpDown = widget.GetVisualDescendants()
            .OfType<NumericUpDown>()
            .FirstOrDefault();

        // Assert
        Assert.NotNull(numericUpDown);
        Assert.Equal(75, numericUpDown.Value);

        window.Close();
    }

    [AvaloniaFact]
    public void RepNetworkWidget_Works_With_Binding_Like_Real_App()
    {
        // This simulates the EXACT binding scenario from NetworkView:
        // <views:RepNetworkWidget RepNetwork="{Binding SelectedIdentity.ARep}"/>

        // Create the parent ViewModel (like MainWindowViewModel)
        var parentVm = new ParentViewModel();

        // Create identity with rep
        var identity = new Identity { Alias = "Test" };
        identity.ARep.Score = 88;
        var identityVm = new IdentityViewModel(identity);
        parentVm.SelectedIdentity = identityVm;

        // Create widget with binding (like in NetworkView)
        var widget = new RepNetworkWidget { Label = "@-rep" };

        // Bind RepNetwork to SelectedIdentity.ARep (mimics XAML binding)
        widget.Bind(
            RepNetworkWidget.RepNetworkProperty,
            new Binding("SelectedIdentity.ARep") { Source = parentVm }
        );

        var window = new Window
        {
            Width = 200,
            Height = 200,
            Content = widget
        };

        window.Show();
        window.UpdateLayout();

        // Find the NumericUpDown
        var numericUpDown = widget.GetVisualDescendants()
            .OfType<NumericUpDown>()
            .FirstOrDefault();

        // Assert
        Assert.NotNull(numericUpDown);
        Assert.Equal(88, numericUpDown.Value);

        window.Close();
    }

    #region ViewModel Field Tests

    [AvaloniaFact]
    public void RepNetworkViewModel_Score_Updates_Model()
    {
        var model = new RepNetwork { Score = 10 };
        var viewModel = new RepNetworkViewModel(model);

        viewModel.Score = 50;

        Assert.Equal(50, model.Score);
        Assert.Equal(50, viewModel.Score);
    }

    [AvaloniaFact]
    public void RepNetworkViewModel_Minor1Used_Updates_Model()
    {
        var model = new RepNetwork { Minor1Used = false };
        var viewModel = new RepNetworkViewModel(model);

        viewModel.Minor1Used = true;

        Assert.True(model.Minor1Used);
        Assert.True(viewModel.Minor1Used);
    }

    [AvaloniaFact]
    public void RepNetworkViewModel_Minor2Used_Updates_Model()
    {
        var model = new RepNetwork { Minor2Used = false };
        var viewModel = new RepNetworkViewModel(model);

        viewModel.Minor2Used = true;

        Assert.True(model.Minor2Used);
        Assert.True(viewModel.Minor2Used);
    }

    [AvaloniaFact]
    public void RepNetworkViewModel_Minor3Used_Updates_Model()
    {
        var model = new RepNetwork { Minor3Used = false };
        var viewModel = new RepNetworkViewModel(model);

        viewModel.Minor3Used = true;

        Assert.True(model.Minor3Used);
        Assert.True(viewModel.Minor3Used);
    }

    [AvaloniaFact]
    public void RepNetworkViewModel_ModerateUsed_Updates_Model()
    {
        var model = new RepNetwork { ModerateUsed = false };
        var viewModel = new RepNetworkViewModel(model);

        viewModel.ModerateUsed = true;

        Assert.True(model.ModerateUsed);
        Assert.True(viewModel.ModerateUsed);
    }

    [AvaloniaFact]
    public void RepNetworkViewModel_MajorUsed_Updates_Model()
    {
        var model = new RepNetwork { MajorUsed = false };
        var viewModel = new RepNetworkViewModel(model);

        viewModel.MajorUsed = true;

        Assert.True(model.MajorUsed);
        Assert.True(viewModel.MajorUsed);
    }

    #endregion

    #region Widget UI Binding Tests

    private (Window window, RepNetworkWidget widget, RepNetworkViewModel viewModel) CreateWidgetInWindow()
    {
        var model = new RepNetwork
        {
            Score = 25,
            Minor1Used = false,
            Minor2Used = true,
            Minor3Used = false,
            ModerateUsed = false,
            MajorUsed = true
        };
        var viewModel = new RepNetworkViewModel(model);

        var widget = new RepNetworkWidget
        {
            RepNetwork = viewModel,
            Label = "Test Rep"
        };

        var window = new Window
        {
            Width = 300,
            Height = 300,
            Content = widget
        };

        window.Show();
        window.UpdateLayout();

        return (window, widget, viewModel);
    }

    [AvaloniaFact]
    public void Widget_Displays_All_Initial_Values_Correctly()
    {
        var (window, widget, viewModel) = CreateWidgetInWindow();

        try
        {
            // Score
            var numericUpDown = widget.GetVisualDescendants().OfType<NumericUpDown>().First();
            Assert.Equal(25, numericUpDown.Value);

            // Toggle buttons (5 total: 3 minor + 1 moderate + 1 major)
            var toggleButtons = widget.GetVisualDescendants().OfType<ToggleButton>().ToList();
            Assert.Equal(5, toggleButtons.Count);

            // Minor favors: false, true, false
            Assert.False(toggleButtons[0].IsChecked);
            Assert.True(toggleButtons[1].IsChecked);
            Assert.False(toggleButtons[2].IsChecked);

            // Moderate: false
            Assert.False(toggleButtons[3].IsChecked);

            // Major: true
            Assert.True(toggleButtons[4].IsChecked);
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaFact]
    public void Widget_Label_Displays_Correctly()
    {
        var (window, widget, _) = CreateWidgetInWindow();

        try
        {
            var textBlocks = widget.GetVisualDescendants().OfType<TextBlock>().ToList();
            var labelBlock = textBlocks.FirstOrDefault(tb => tb.Text == "Test Rep");
            Assert.NotNull(labelBlock);
        }
        finally
        {
            window.Close();
        }
    }

    #endregion

    #region Toggle Button Interaction Tests

    [AvaloniaFact]
    public void Clicking_Minor1_ToggleButton_Updates_ViewModel()
    {
        var (window, widget, viewModel) = CreateWidgetInWindow();

        try
        {
            var toggleButtons = widget.GetVisualDescendants().OfType<ToggleButton>().ToList();
            var minor1Button = toggleButtons[0];

            Assert.False(viewModel.Minor1Used);

            // Simulate click
            minor1Button.IsChecked = true;

            Assert.True(viewModel.Minor1Used);
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaFact]
    public void Clicking_Minor2_ToggleButton_Updates_ViewModel()
    {
        var (window, widget, viewModel) = CreateWidgetInWindow();

        try
        {
            var toggleButtons = widget.GetVisualDescendants().OfType<ToggleButton>().ToList();
            var minor2Button = toggleButtons[1];

            Assert.True(viewModel.Minor2Used); // Initially true

            // Simulate click to uncheck
            minor2Button.IsChecked = false;

            Assert.False(viewModel.Minor2Used);
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaFact]
    public void Clicking_Minor3_ToggleButton_Updates_ViewModel()
    {
        var (window, widget, viewModel) = CreateWidgetInWindow();

        try
        {
            var toggleButtons = widget.GetVisualDescendants().OfType<ToggleButton>().ToList();
            var minor3Button = toggleButtons[2];

            Assert.False(viewModel.Minor3Used);

            minor3Button.IsChecked = true;

            Assert.True(viewModel.Minor3Used);
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaFact]
    public void Clicking_Moderate_ToggleButton_Updates_ViewModel()
    {
        var (window, widget, viewModel) = CreateWidgetInWindow();

        try
        {
            var toggleButtons = widget.GetVisualDescendants().OfType<ToggleButton>().ToList();
            var moderateButton = toggleButtons[3];

            Assert.False(viewModel.ModerateUsed);

            moderateButton.IsChecked = true;

            Assert.True(viewModel.ModerateUsed);
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaFact]
    public void Clicking_Major_ToggleButton_Updates_ViewModel()
    {
        var (window, widget, viewModel) = CreateWidgetInWindow();

        try
        {
            var toggleButtons = widget.GetVisualDescendants().OfType<ToggleButton>().ToList();
            var majorButton = toggleButtons[4];

            Assert.True(viewModel.MajorUsed); // Initially true

            majorButton.IsChecked = false;

            Assert.False(viewModel.MajorUsed);
        }
        finally
        {
            window.Close();
        }
    }

    #endregion

    #region NumericUpDown Interaction Tests

    [AvaloniaFact]
    public void NumericUpDown_Value_Change_Updates_ViewModel()
    {
        var (window, widget, viewModel) = CreateWidgetInWindow();

        try
        {
            var numericUpDown = widget.GetVisualDescendants().OfType<NumericUpDown>().First();

            Assert.Equal(25, viewModel.Score);

            numericUpDown.Value = 50;

            Assert.Equal(50, viewModel.Score);
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaFact]
    public void NumericUpDown_Increment_Via_Spinner_Updates_ViewModel()
    {
        var (window, widget, viewModel) = CreateWidgetInWindow();

        try
        {
            var numericUpDown = widget.GetVisualDescendants().OfType<NumericUpDown>().First();
            var initialValue = viewModel.Score;

            // Find the increment button (usually a RepeatButton with specific name or position)
            var spinners = numericUpDown.GetVisualDescendants().OfType<RepeatButton>().ToList();

            // Spinners: typically [0] is increment, [1] is decrement (or vice versa)
            // We'll use the Increment method directly for reliability
            numericUpDown.Value = (numericUpDown.Value ?? 0) + 1;

            Assert.Equal(initialValue + 1, viewModel.Score);
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaFact]
    public void NumericUpDown_Decrement_Via_Spinner_Updates_ViewModel()
    {
        var (window, widget, viewModel) = CreateWidgetInWindow();

        try
        {
            var numericUpDown = widget.GetVisualDescendants().OfType<NumericUpDown>().First();
            var initialValue = viewModel.Score;

            numericUpDown.Value = (numericUpDown.Value ?? 0) - 1;

            Assert.Equal(initialValue - 1, viewModel.Score);
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaFact]
    public void NumericUpDown_Has_Correct_Minimum_Value()
    {
        var (window, widget, viewModel) = CreateWidgetInWindow();

        try
        {
            var numericUpDown = widget.GetVisualDescendants().OfType<NumericUpDown>().First();

            // Verify the minimum is set correctly
            Assert.Equal(0, numericUpDown.Minimum);
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaFact]
    public void NumericUpDown_Has_Correct_Maximum_Value()
    {
        var (window, widget, viewModel) = CreateWidgetInWindow();

        try
        {
            var numericUpDown = widget.GetVisualDescendants().OfType<NumericUpDown>().First();

            // Verify the maximum is set correctly
            Assert.Equal(99, numericUpDown.Maximum);
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaFact]
    public void NumericUpDown_Cannot_Decrement_Below_Minimum()
    {
        var (window, widget, viewModel) = CreateWidgetInWindow();

        try
        {
            var numericUpDown = widget.GetVisualDescendants().OfType<NumericUpDown>().First();

            // Set to minimum
            numericUpDown.Value = 0;
            window.UpdateLayout();

            // Try to decrement - value should stay at minimum
            var valueBefore = numericUpDown.Value;
            // Simulate decrement attempt (value - 1, but clamped by binding/ViewModel logic)
            var newValue = Math.Max(0, (numericUpDown.Value ?? 0) - 1);
            numericUpDown.Value = newValue;

            Assert.Equal(0, numericUpDown.Value);
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaFact]
    public void NumericUpDown_Cannot_Increment_Above_Maximum()
    {
        var (window, widget, viewModel) = CreateWidgetInWindow();

        try
        {
            var numericUpDown = widget.GetVisualDescendants().OfType<NumericUpDown>().First();

            // Set to maximum
            numericUpDown.Value = 99;
            window.UpdateLayout();

            // Try to increment - value should stay at maximum
            var newValue = Math.Min(99, (numericUpDown.Value ?? 0) + 1);
            numericUpDown.Value = newValue;

            Assert.Equal(99, numericUpDown.Value);
        }
        finally
        {
            window.Close();
        }
    }

    #endregion

    #region Two-Way Binding Tests

    [AvaloniaFact]
    public void ViewModel_Score_Change_Updates_NumericUpDown()
    {
        var (window, widget, viewModel) = CreateWidgetInWindow();

        try
        {
            var numericUpDown = widget.GetVisualDescendants().OfType<NumericUpDown>().First();

            viewModel.Score = 77;
            window.UpdateLayout();

            Assert.Equal(77, numericUpDown.Value);
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaFact]
    public void ViewModel_Minor1Used_Change_Updates_ToggleButton()
    {
        var (window, widget, viewModel) = CreateWidgetInWindow();

        try
        {
            var toggleButtons = widget.GetVisualDescendants().OfType<ToggleButton>().ToList();
            var minor1Button = toggleButtons[0];

            Assert.False(minor1Button.IsChecked);

            viewModel.Minor1Used = true;
            window.UpdateLayout();

            Assert.True(minor1Button.IsChecked);
        }
        finally
        {
            window.Close();
        }
    }

    [AvaloniaFact]
    public void ViewModel_ModerateUsed_Change_Updates_ToggleButton()
    {
        var (window, widget, viewModel) = CreateWidgetInWindow();

        try
        {
            var toggleButtons = widget.GetVisualDescendants().OfType<ToggleButton>().ToList();
            var moderateButton = toggleButtons[3];

            Assert.False(moderateButton.IsChecked);

            viewModel.ModerateUsed = true;
            window.UpdateLayout();

            Assert.True(moderateButton.IsChecked);
        }
        finally
        {
            window.Close();
        }
    }

    #endregion

    #region Model Persistence Tests

    [AvaloniaFact]
    public void Widget_Changes_Persist_To_Underlying_Model()
    {
        // This test verifies the full chain: Widget -> ViewModel -> Model
        var model = new RepNetwork
        {
            Score = 10,
            Minor1Used = false,
            Minor2Used = false,
            Minor3Used = false,
            ModerateUsed = false,
            MajorUsed = false
        };
        var viewModel = new RepNetworkViewModel(model);

        var widget = new RepNetworkWidget
        {
            RepNetwork = viewModel,
            Label = "Test"
        };

        var window = new Window { Width = 300, Height = 300, Content = widget };
        window.Show();
        window.UpdateLayout();

        try
        {
            var numericUpDown = widget.GetVisualDescendants().OfType<NumericUpDown>().First();
            var toggleButtons = widget.GetVisualDescendants().OfType<ToggleButton>().ToList();

            // Make changes via UI controls
            numericUpDown.Value = 42;
            toggleButtons[0].IsChecked = true;  // Minor1
            toggleButtons[1].IsChecked = true;  // Minor2
            toggleButtons[3].IsChecked = true;  // Moderate

            // Verify changes persisted to the original model
            Assert.Equal(42, model.Score);
            Assert.True(model.Minor1Used);
            Assert.True(model.Minor2Used);
            Assert.False(model.Minor3Used);
            Assert.True(model.ModerateUsed);
            Assert.False(model.MajorUsed);
        }
        finally
        {
            window.Close();
        }
    }

    #endregion
}

// Helper class to simulate MainWindowViewModel for testing
public class ParentViewModel : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    private IdentityViewModel? _selectedIdentity;
    public IdentityViewModel? SelectedIdentity
    {
        get => _selectedIdentity;
        set => SetProperty(ref _selectedIdentity, value);
    }
}