using System.Text.Json.Serialization;

namespace EPTools.Core.Models.EPDataModels;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(GearWare), typeDiscriminator: "ware")]
[JsonDerivedType(typeof(GearArmor), typeDiscriminator: "armor")]
[JsonDerivedType(typeof(GearBot), typeDiscriminator: "bot")]
[JsonDerivedType(typeof(GearComms), typeDiscriminator: "comms")]
[JsonDerivedType(typeof(GearCreature), typeDiscriminator: "creature")]
[JsonDerivedType(typeof(GearDrug), typeDiscriminator: "drug")]
[JsonDerivedType(typeof(GearItem), typeDiscriminator: "item")]
[JsonDerivedType(typeof(GearMission), typeDiscriminator: "mission")]
[JsonDerivedType(typeof(GearNano), typeDiscriminator: "nano")]
[JsonDerivedType(typeof(GearSecurity), typeDiscriminator: "security")]
[JsonDerivedType(typeof(GearService), typeDiscriminator: "service")]
[JsonDerivedType(typeof(GearSoftware), typeDiscriminator: "software")]
[JsonDerivedType(typeof(GearSwarm), typeDiscriminator: "swarm")]
[JsonDerivedType(typeof(GearVehicle), typeDiscriminator: "vehicle")]
[JsonDerivedType(typeof(GearWeaponAmmo), typeDiscriminator: "ammo")]
[JsonDerivedType(typeof(GearWeaponMelee), typeDiscriminator: "melee")]
[JsonDerivedType(typeof(GearWeaponRanged), typeDiscriminator: "ranged")]
public abstract class Gear : EpModel
{
    public string Category { get; set; } = string.Empty;
    public string Subcategory { get; set; } = string.Empty;
    public string Complexity { get; set; } = string.Empty;
}