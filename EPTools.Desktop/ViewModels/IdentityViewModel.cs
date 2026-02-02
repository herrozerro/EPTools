using System;
using CommunityToolkit.Mvvm.ComponentModel;
using EPTools.Core.Models.Ego;

namespace EPTools.Desktop.ViewModels;

public partial class IdentityViewModel : ObservableObject
{
    private readonly Identity _model;

    public IdentityViewModel(Identity model)
    {
        _model = model;
        ARep = new RepNetworkViewModel(model.ARep);
        CRep = new RepNetworkViewModel(model.CRep);
        FRep = new RepNetworkViewModel(model.FRep);
        GRep = new RepNetworkViewModel(model.GRep);
        IRep = new RepNetworkViewModel(model.IRep);
        RRep = new RepNetworkViewModel(model.RRep);
        XRep = new RepNetworkViewModel(model.XRep);
    }

    public Identity Model => _model;

    public Guid Id => _model.Id;

    public string Alias
    {
        get => _model.Alias;
        set => SetProperty(_model.Alias, value, _model, (m, v) => m.Alias = v);
    }

    public string Location
    {
        get => _model.Location;
        set => SetProperty(_model.Location, value, _model, (m, v) => m.Location = v);
    }

    public RepNetworkViewModel ARep { get; }
    public RepNetworkViewModel CRep { get; }
    public RepNetworkViewModel FRep { get; }
    public RepNetworkViewModel GRep { get; }
    public RepNetworkViewModel IRep { get; }
    public RepNetworkViewModel RRep { get; }
    public RepNetworkViewModel XRep { get; }
}
