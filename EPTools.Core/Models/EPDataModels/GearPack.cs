namespace EPTools.Core.Models.EPDataModels;

public record GearPack(
    int Name,
    int Type,
    List<string> Gear,
    List<GearPackOption> Options,
    int Resource,
    int Reference,
    int Id
);

public record GearPackOption(
    int Name,
    List<string> Gear
);