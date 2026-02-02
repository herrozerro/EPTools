using CommunityToolkit.Mvvm.ComponentModel;
using EPTools.Core.Models.Ego;

namespace EPTools.Desktop.ViewModels;

public partial class RepNetworkViewModel : ObservableObject
{
    private readonly RepNetwork _model;

    public RepNetworkViewModel(RepNetwork model)
    {
        _model = model;
    }

    public RepNetwork Model => _model;

    public int Score
    {
        get => _model.Score;
        set => SetProperty(_model.Score, value, _model, (m, v) => m.Score = v);
    }

    public bool Minor1Used
    {
        get => _model.Minor1Used;
        set => SetProperty(_model.Minor1Used, value, _model, (m, v) => m.Minor1Used = v);
    }

    public bool Minor2Used
    {
        get => _model.Minor2Used;
        set => SetProperty(_model.Minor2Used, value, _model, (m, v) => m.Minor2Used = v);
    }

    public bool Minor3Used
    {
        get => _model.Minor3Used;
        set => SetProperty(_model.Minor3Used, value, _model, (m, v) => m.Minor3Used = v);
    }

    public bool ModerateUsed
    {
        get => _model.ModerateUsed;
        set => SetProperty(_model.ModerateUsed, value, _model, (m, v) => m.ModerateUsed = v);
    }

    public bool MajorUsed
    {
        get => _model.MajorUsed;
        set => SetProperty(_model.MajorUsed, value, _model, (m, v) => m.MajorUsed = v);
    }
}
