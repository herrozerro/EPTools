#pragma warning disable CS8618

namespace EPTools.Core.Models.Ego;

public class Identity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Alias { get; set; }
    public string Location { get; set; }

    // Initialize these so they aren't null
    public RepNetwork ARep { get; set; } = new();
    public RepNetwork CRep { get; set; } = new();
    public RepNetwork FRep { get; set; } = new();
    public RepNetwork GRep { get; set; } = new();
    // ReSharper disable once InconsistentNaming
    public RepNetwork IRep { get; set; } = new();
    public RepNetwork RRep { get; set; } = new();
    public RepNetwork XRep { get; set; } = new();
}

public class RepNetwork
{
    public int Score { get; set; }

    // Favor Tracking (True = Used/Burned, False = Available)
    public bool Minor1Used { get; set; }
    public bool Minor2Used { get; set; }
    public bool Minor3Used { get; set; }
    public bool ModerateUsed { get; set; }
    public bool MajorUsed { get; set; }
}