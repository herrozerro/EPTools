namespace EPTools.Core.Models.EPDataModels;

public record GearPack(
    string Name,
    string Type,
    List<string> Gear,
    List<GearPackOption> Options,
    string Resource,
    string Reference,
    int Id
);

public record GearPackOption(
    string Name,
    List<string> Gear
);