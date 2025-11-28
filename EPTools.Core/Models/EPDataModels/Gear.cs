using System.Text.Json.Serialization;

namespace EPTools.Core.Models.EPDataModels
{
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
    [JsonDerivedType(typeof(GearSoftware), typeDiscriminator: "software")]
    [JsonDerivedType(typeof(GearSwarm), typeDiscriminator: "swarm")]
    [JsonDerivedType(typeof(GearVehicle), typeDiscriminator: "vehicle")]
    [JsonDerivedType(typeof(GearWeaponAmmo), typeDiscriminator: "ammo")]
    [JsonDerivedType(typeof(GearWeaponMelee), typeDiscriminator: "melee")]
    [JsonDerivedType(typeof(GearWeaponRanged), typeDiscriminator: "ranged")]
    public abstract record Gear(
        string Category,
        string Subcategory,
        string Name,
        string Complexity,
        string Description,
        string Summary,
        string Resource,
        string Reference,
        List<AdditionalRules> AdditionalRules);

    public record AdditionalRules(
        string Name,
        string Description,
        AdditionalRuleType Type,
        int Value);
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AdditionalRuleType {
        SkillBonus,
        SkillAdded,
        AptitudeCheck,
        Pool,
        Recharge,
        Armor,
        Weapon,
        WoundThreshold,
        Durability,
        TraumaThreshold,
        Lucidity,
        InsanityRating,
        IgnoreWound,
        IgnoreTrauma,
        DamageValueAdded,
        MovementRate,
        MovementRateFull
    }
}