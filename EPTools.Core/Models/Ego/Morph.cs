namespace EPTools.Core.Models.Ego;

public class Morph
{
    public string Name { get; set; } = string.Empty;
    public string MorphType { get; set; } = string.Empty;
    public string MorphSex { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public bool ActiveMorph { get; set; }

    // Base pool values from the morph template
    public int Vigor { get; set; }
    public int Insight { get; set; }
    public int Moxie { get; set; }
    public int MorphFlex { get; set; }

    // Base durability stats from the morph template
    public int Durability { get; set; }
    public int WoundThreshold { get; set; }
    public int DeathRating { get; set; }
    public int ArmorEnergy { get; set; }
    public int ArmorKinetic { get; set; }

    public List<RollModifier> RollModifiers { get; set; } = [];
    public List<EgoTrait> Traits { get; set; } = [];
    public List<Ware> Wares { get; set; } = [];

    // Computed totals — base value plus any active modifiers
    public int TotalVigor        => Vigor         + ActiveModifiers(RollModifierType.VigorPool);
    public int TotalInsight      => Insight        + ActiveModifiers(RollModifierType.InsightPool);
    public int TotalMoxie        => Moxie          + ActiveModifiers(RollModifierType.MoxiePool);
    public int TotalFlex         => MorphFlex      + ActiveModifiers(RollModifierType.FlexPool);
    public int TotalDurability   => Durability     + ActiveModifiers(RollModifierType.Durability);
    public int TotalWoundThreshold => WoundThreshold + ActiveModifiers(RollModifierType.WoundThreshold);
    public int TotalDeathRating  => DeathRating    + ActiveModifiers(RollModifierType.DeathRating);
    public int TotalArmorEnergy  => ArmorEnergy    + ActiveModifiers(RollModifierType.ArmorEnergy);
    public int TotalArmorKinetic => ArmorKinetic   + ActiveModifiers(RollModifierType.ArmorKinetic);

    private int ActiveModifiers(RollModifierType type) =>
        RollModifiers.Where(x => x.IsActive && x.Type == type).Sum(x => x.Value);
}
