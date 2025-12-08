namespace EPTools.Core.Models.EPDataModels;

public class Morph : EpModel
{
    public string Type { get; set; } = string.Empty;
    public int Cost { get; set; }
    public int Availability { get; set; } 
    public int WoundThreshold { get; set; } 
    public int Durability { get; set; } 
    public int DeathRating { get; set; }
    public MorphPools Pools { get; set; }
    public List<MovementRates> MovementRate { get; set; } = [];
    public List<string> Ware { get; set; } = [];
    public List<MorphTrait> MorphTraits { get; set; } = [];
    public List<string> CommonExtras { get; set; } = [];
    public List<string> Notes { get; set; } = [];
    public List<string> CommonShapeAdjustments { get; set; } = [];
}

public class MorphPools
{
    public int Insight { get; set; }
    public int Moxie { get; set; }
    public int Vigor { get; set; }
    public int Flex { get; set; }
}

public class MovementRates
{
    public string MovementType { get; set; }  = string.Empty;
    public int Base { get; set; }
    public int Full { get; set; }
}

public class MorphTrait
{
    public string Name { get; set; } = string.Empty;
    public int Level { get; set; }
}