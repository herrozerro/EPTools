using CommunityToolkit.Mvvm.ComponentModel;
using EPTools.Core.Models.Ego;

namespace EpTools.Desktop.ViewModels;

public partial class EgoAptitudeViewModel : ObservableObject
{
    private readonly EgoAptitude _model;

    public EgoAptitudeViewModel(EgoAptitude model)
    {
        _model = model;
    }

    public EgoAptitude Model => _model;

    public string Name
    {
        get => _model.Name;
        set => SetProperty(_model.Name, value, _model, (m, v) => m.Name = v);
    }

    public string ShortName
    {
        get => _model.ShortName;
        set => SetProperty(_model.ShortName, value, _model, (m, v) => m.ShortName = v);
    }

    public int AptitudeValue
    {
        get => _model.AptitudeValue;
        set
        {
            if (SetProperty(_model.AptitudeValue, value, _model, (m, v) => m.AptitudeValue = v))
            {
                OnPropertyChanged(nameof(CheckRating));
            }
        }
    }

    public int CheckMod
    {
        get => _model.CheckMod;
        set
        {
            if (SetProperty(_model.CheckMod, value, _model, (m, v) => m.CheckMod = v))
            {
                OnPropertyChanged(nameof(CheckRating));
            }
        }
    }

    public int CheckRating => _model.CheckRating;
}
