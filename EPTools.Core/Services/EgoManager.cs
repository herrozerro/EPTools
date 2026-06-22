using EPTools.Core.Constants;
using EPTools.Core.Models.Data;
using EPTools.Core.Models.Ego;

namespace EPTools.Core.Services;

/// <summary>
/// Manages mutations to an Ego, providing a centralized API for adding, removing,
/// and modifying character data. This ensures consistent business rules across all clients.
/// </summary>
public class EgoManager
{
    /// <summary>
    /// Valid aptitudes for knowledge skills (COG or INT only).
    /// </summary>
    public static readonly IReadOnlyList<string> KnowledgeSkillAptitudes = new[]
    {
        AptitudeNames.Cognition,
        AptitudeNames.Intuition
    };

    /// <summary>
    /// All valid aptitudes for exotic skills.
    /// </summary>
    public static readonly IReadOnlyList<string> AllAptitudes = new[]
    {
        AptitudeNames.Cognition,
        AptitudeNames.Intuition,
        AptitudeNames.Reflexes,
        AptitudeNames.Savvy,
        AptitudeNames.Somatics,
        AptitudeNames.Willpower
    };

    #region Skills

    /// <summary>
    /// Adds a new knowledge skill to the ego.
    /// </summary>
    public EgoSkill AddKnowledgeSkill(Ego ego, string name = "New Knowledge Skill", string aptitude = "Intuition")
    {
        if (!KnowledgeSkillAptitudes.Contains(aptitude))
        {
            throw new ArgumentException($"Knowledge skills must use {string.Join(" or ", KnowledgeSkillAptitudes)}", nameof(aptitude));
        }

        var skill = new EgoSkill
        {
            Name = name,
            SkillType = SkillType.KnowledgeSkill,
            Aptitude = aptitude
        };
        ego.Skills.Add(skill);
        return skill;
    }

    /// <summary>
    /// Adds a new exotic skill to the ego.
    /// </summary>
    public EgoSkill AddExoticSkill(Ego ego, string name = "New Exotic Skill", string aptitude = "Reflexes")
    {
        if (!AllAptitudes.Contains(aptitude))
        {
            throw new ArgumentException($"Invalid aptitude: {aptitude}", nameof(aptitude));
        }

        var skill = new EgoSkill
        {
            Name = name,
            SkillType = SkillType.ExoticSkill,
            Aptitude = aptitude
        };
        ego.Skills.Add(skill);
        return skill;
    }

    /// <summary>
    /// Removes a skill from the ego. Active (ego) skills cannot be removed.
    /// </summary>
    /// <returns>True if the skill was removed, false if it couldn't be removed.</returns>
    public bool RemoveSkill(Ego ego, EgoSkill skill)
    {
        if (skill.SkillType == SkillType.EgoSkill)
        {
            return false; // Cannot remove active skills
        }

        return ego.Skills.Remove(skill);
    }

    /// <summary>
    /// Changes the aptitude linked to a skill. Only valid for knowledge and exotic skills.
    /// Knowledge skills are restricted to COG or INT.
    /// </summary>
    public bool SetSkillAptitude(Ego ego, EgoSkill skill, string aptitude)
    {
        if (skill.SkillType == SkillType.EgoSkill)
        {
            return false; // Cannot change aptitude for active skills
        }

        if (skill.SkillType == SkillType.KnowledgeSkill && !KnowledgeSkillAptitudes.Contains(aptitude))
        {
            return false; // Knowledge skills restricted to COG/INT
        }

        if (!AllAptitudes.Contains(aptitude))
        {
            return false;
        }

        skill.Aptitude = aptitude;
        return true;
    }

    #endregion

    #region Morphs

    /// <summary>
    /// Instantiates a morph from a template, clears existing morphs, and sets it as active.
    /// Used by lifepath generation to assign the character's starting morph.
    /// </summary>
    public Morph ApplyMorphTemplate(Ego ego, MorphTemplate template)
    {
        ego.Morphs.Clear();
        var morph = new Morph
        {
            Name = template.Name,
            MorphType = template.Type,
            MorphSex = "",
            ActiveMorph = true,
            Insight = template.Pools.Insight,
            Moxie = template.Pools.Moxie,
            Vigor = template.Pools.Vigor,
            MorphFlex = template.Pools.Flex,
            Durability = template.Durability,
            WoundThreshold = template.WoundThreshold,
            DeathRating = template.DeathRating,
            Traits = template.MorphTraits.Select(x => new EgoTrait { Name = x.Name, Level = x.Level }).ToList(),
            Wares = template.Ware.Select(x => new Ware { Name = x }).ToList()
        };
        ego.Morphs.Add(morph);
        return morph;
    }

    /// <summary>
    /// Adds a new morph to the ego. If this is the first morph, it becomes active automatically.
    /// </summary>
    public Morph AddMorph(Ego ego, string name = "New Morph", string morphType = "Biomorph", string size = "Medium")
    {
        var morph = new Morph
        {
            Name = name,
            MorphType = morphType,
            Size = size,
            ActiveMorph = ego.Morphs.Count == 0 // First morph is active by default
        };
        ego.Morphs.Add(morph);
        return morph;
    }

    /// <summary>
    /// Removes a morph from the ego. If the removed morph was active and other morphs exist,
    /// the first remaining morph becomes active.
    /// </summary>
    /// <returns>True if the morph was removed, false if it's the last morph.</returns>
    public bool RemoveMorph(Ego ego, Morph morph)
    {
        if (ego.Morphs.Count <= 1)
        {
            return false; // Keep at least one morph
        }

        var wasActive = morph.ActiveMorph;
        ego.Morphs.Remove(morph);

        // If deleted morph was active, make another one active
        if (wasActive && ego.Morphs.Count > 0)
        {
            ego.Morphs[0].ActiveMorph = true;
        }

        return true;
    }

    /// <summary>
    /// Sets the specified morph as the active morph, deactivating all others.
    /// </summary>
    public void SetActiveMorph(Ego ego, Morph morph)
    {
        foreach (var m in ego.Morphs)
        {
            m.ActiveMorph = false;
        }
        morph.ActiveMorph = true;
    }

    /// <summary>
    /// Gets the currently active morph, or null if none exists.
    /// </summary>
    public Morph? GetActiveMorph(Ego ego)
    {
        return ego.Morphs.FirstOrDefault(m => m.ActiveMorph);
    }

    #endregion

    #region Morph Traits

    /// <summary>
    /// Adds a trait to a morph.
    /// </summary>
    public EgoTrait AddMorphTrait(Morph morph, string name = "New Trait")
    {
        var trait = new EgoTrait { Name = name };
        morph.Traits.Add(trait);
        return trait;
    }

    /// <summary>
    /// Removes a trait from a morph.
    /// </summary>
    public bool RemoveMorphTrait(Morph morph, EgoTrait trait)
    {
        return morph.Traits.Remove(trait);
    }

    #endregion

    #region Morph Ware

    /// <summary>
    /// Adds ware to a morph.
    /// </summary>
    public Ware AddMorphWare(Morph morph, string name = "New Ware", int quantity = 1)
    {
        var ware = new Ware { Name = name, Quantity = quantity };
        morph.Wares.Add(ware);
        return ware;
    }

    /// <summary>
    /// Removes ware from a morph.
    /// </summary>
    public bool RemoveMorphWare(Morph morph, Ware ware)
    {
        return morph.Wares.Remove(ware);
    }

    #endregion

    #region Psi

    public EgoSleight AddSleight(Ego ego, EgoSleight sleight)
    {
        ego.Psi.Sleights.Add(sleight);
        return sleight;
    }

    #endregion

    #region Identities

    /// <summary>
    /// Adds a new identity to the ego.
    /// </summary>
    public Identity AddIdentity(Ego ego, string alias = "New Identity")
    {
        var identity = new Identity { Alias = alias };
        ego.Identities.Add(identity);
        return identity;
    }

    public void AddIdentity(Ego ego, Identity identity)
    {
        ego.Identities.Add(identity);
    }

    /// <summary>
    /// Adds rep to a specific network on an identity. Returns false if the network name is unknown.
    /// </summary>
    public bool AddReputation(Identity identity, string network, int amount)
    {
        var rep = network switch
        {
            "ARep" => identity.ARep,
            "CRep" => identity.CRep,
            "FRep" => identity.FRep,
            "GRep" => identity.GRep,
            "IRep" => identity.IRep,
            "RRep" => identity.RRep,
            "XRep" => identity.XRep,
            _ => null
        };
        if (rep == null) return false;
        rep.Score += amount;
        return true;
    }

    /// <summary>
    /// Removes an identity from the ego.
    /// </summary>
    public bool RemoveIdentity(Ego ego, Identity identity)
    {
        if (ego.Identities.Count <= 1)
        {
            return false; // Keep at least one identity
        }
        return ego.Identities.Remove(identity);
    }

    #endregion

    #region Ego Traits

    /// <summary>
    /// Adds a pre-built trait to the ego (used when applying from a catalog or lifepath).
    /// </summary>
    public EgoTrait AddEgoTrait(Ego ego, EgoTrait trait)
    {
        ego.EgoTraits.Add(trait);
        return trait;
    }

    /// <summary>
    /// Adds a blank trait to the ego.
    /// </summary>
    public EgoTrait AddEgoTrait(Ego ego, string name = "New Trait")
    {
        var trait = new EgoTrait { Name = name };
        ego.EgoTraits.Add(trait);
        return trait;
    }

    /// <summary>
    /// Removes a trait from the ego.
    /// </summary>
    public bool RemoveEgoTrait(Ego ego, EgoTrait trait)
    {
        return ego.EgoTraits.Remove(trait);
    }

    #endregion

    #region Inventory

    /// <summary>
    /// Adds a pre-built item to the ego's local inventory.
    /// </summary>
    public InventoryItem AddInventoryItem(Ego ego, InventoryItem item)
    {
        ego.Inventory.Add(item);
        return item;
    }

    /// <summary>
    /// Adds a blank item to the ego's local inventory.
    /// </summary>
    public InventoryItem AddInventoryItem(Ego ego, string name = "New Item", int quantity = 1)
    {
        var item = new InventoryItem { Name = name, Quantity = quantity };
        ego.Inventory.Add(item);
        return item;
    }

    /// <summary>
    /// Removes an item from the ego's local inventory.
    /// </summary>
    public bool RemoveInventoryItem(Ego ego, InventoryItem item)
    {
        return ego.Inventory.Remove(item);
    }

    /// <summary>
    /// Moves an item from local inventory to a cache. Resets equipped/active state
    /// since a cached item cannot be in use.
    /// </summary>
    public bool MoveItemToCache(Ego ego, InventoryItem item, InventoryCache cache)
    {
        if (!ego.Inventory.Remove(item))
            return false;

        item.Equipped = false;
        item.Active = false;
        cache.Inventory.Add(item);
        return true;
    }

    /// <summary>
    /// Moves an item from a cache to local inventory. Resets equipped/active state
    /// so the player consciously re-equips after retrieving.
    /// </summary>
    public bool MoveItemFromCache(Ego ego, InventoryItem item, InventoryCache cache)
    {
        if (!cache.Inventory.Remove(item))
            return false;

        item.Equipped = false;
        item.Active = false;
        ego.Inventory.Add(item);
        return true;
    }

    #endregion

    #region Inventory Caches

    /// <summary>
    /// Adds a new inventory cache (storage location).
    /// </summary>
    public InventoryCache AddCache(Ego ego, string location = "New Cache")
    {
        var cache = new InventoryCache { Location = location };
        ego.Caches.Add(cache);
        return cache;
    }

    /// <summary>
    /// Removes an inventory cache. Items in the cache are lost.
    /// </summary>
    public bool RemoveCache(Ego ego, InventoryCache cache)
    {
        return ego.Caches.Remove(cache);
    }

    /// <summary>
    /// Adds a pre-built item directly to a cache.
    /// </summary>
    public InventoryItem AddItemToCache(InventoryCache cache, InventoryItem item)
    {
        cache.Inventory.Add(item);
        return item;
    }

    /// <summary>
    /// Adds a blank item directly to a cache.
    /// </summary>
    public InventoryItem AddItemToCache(InventoryCache cache, string name = "New Item", int quantity = 1)
    {
        var item = new InventoryItem { Name = name, Quantity = quantity };
        cache.Inventory.Add(item);
        return item;
    }

    /// <summary>
    /// Removes an item from a cache.
    /// </summary>
    public bool RemoveItemFromCache(InventoryCache cache, InventoryItem item)
    {
        return cache.Inventory.Remove(item);
    }

    #endregion

    #region Roll Modifiers

    public RollModifier AddEgoModifier(Ego ego, RollModifier modifier)
    {
        ego.RollModifiers.Add(modifier);
        return modifier;
    }

    public bool RemoveEgoModifier(Ego ego, RollModifier modifier) =>
        ego.RollModifiers.Remove(modifier);

    /// <summary>
    /// Removes all ego modifiers created by a specific source (e.g. when a trait is removed).
    /// </summary>
    public void RemoveEgoModifiersBySource(Ego ego, string source) =>
        ego.RollModifiers.RemoveAll(x => x.Source == source);

    public RollModifier AddMorphModifier(Morph morph, RollModifier modifier)
    {
        morph.RollModifiers.Add(modifier);
        return modifier;
    }

    public bool RemoveMorphModifier(Morph morph, RollModifier modifier) =>
        morph.RollModifiers.Remove(modifier);

    /// <summary>
    /// Removes all morph modifiers created by a specific source (e.g. when a ware is removed).
    /// </summary>
    public void RemoveMorphModifiersBySource(Morph morph, string source) =>
        morph.RollModifiers.RemoveAll(x => x.Source == source);

    #endregion

    #region Calculations

    /// <summary>
    /// Full skill total: rank + linked aptitude + active modifiers from both ego and morph.
    /// Skill modifiers match by skill name; aptitude-check modifiers match by the skill's linked aptitude.
    /// </summary>
    public int GetSkillTotal(Ego ego, EgoSkill skill, Morph? activeMorph = null)
    {
        var baseTotal = ego.SkillTotal(skill);
        var egoMods   = SumActive(ego.RollModifiers, RollModifierType.Skill, skill.Name);
        var morphMods = activeMorph == null ? 0
                      : SumActive(activeMorph.RollModifiers, RollModifierType.Skill, skill.Name);
        return baseTotal + egoMods + morphMods;
    }

    /// <summary>
    /// Aptitude check total: (aptitude * 3 + checkMod) + active AptitudeCheck modifiers from both ego and morph.
    /// Match modifiers by full aptitude name (e.g. "Cognition") stored in RollModifier.Name.
    /// </summary>
    public int GetAptitudeCheckTotal(Ego ego, string aptitudeName, Morph? activeMorph = null)
    {
        var aptitude = ego.Aptitudes.Find(x => x.Name == aptitudeName || x.ShortName == aptitudeName);
        if (aptitude == null) return 0;

        var egoMods   = SumActive(ego.RollModifiers, RollModifierType.AptitudeCheck, aptitude.Name);
        var morphMods = activeMorph == null ? 0
                      : SumActive(activeMorph.RollModifiers, RollModifierType.AptitudeCheck, aptitude.Name);
        return aptitude.CheckRating + egoMods + morphMods;
    }

    /// <summary>
    /// Initiative: (REF + INT) / 5 + active modifiers from both ego and morph.
    /// </summary>
    public int GetInitiative(Ego ego, Morph? activeMorph = null)
    {
        var reflexes  = ego.Aptitudes.Find(x => x.Name == AptitudeNames.Reflexes)?.AptitudeValue ?? 0;
        var intuition = ego.Aptitudes.Find(x => x.Name == AptitudeNames.Intuition)?.AptitudeValue ?? 0;
        var egoMods   = SumActive(ego.RollModifiers, RollModifierType.Initiative);
        var morphMods = activeMorph == null ? 0 : SumActive(activeMorph.RollModifiers, RollModifierType.Initiative);
        return (reflexes + intuition) / 5 + egoMods + morphMods;
    }

    public int GetLucidity(Ego ego)
    {
        var willpower = ego.Aptitudes.Find(x => x.Name == AptitudeNames.Willpower)?.AptitudeValue ?? 0;
        return willpower * 2 + SumActive(ego.RollModifiers, RollModifierType.Lucidity);
    }

    public int GetTraumaThreshold(Ego ego) =>
        GetLucidity(ego) / 5 + SumActive(ego.RollModifiers, RollModifierType.TraumaThreshold);

    public int GetInsanityRating(Ego ego) =>
        GetLucidity(ego) * 2 + SumActive(ego.RollModifiers, RollModifierType.InsanityRating);

    /// <summary>
    /// Gets the total pools combining ego flex and the active morph's modified pool values.
    /// </summary>
    public (int Insight, int Moxie, int Vigor, int Flex) GetTotalPools(Ego ego)
    {
        var activeMorph = GetActiveMorph(ego);
        return (
            Insight: activeMorph?.TotalInsight ?? 0,
            Moxie:   activeMorph?.TotalMoxie   ?? 0,
            Vigor:   activeMorph?.TotalVigor   ?? 0,
            Flex:    ego.EgoFlex + (activeMorph?.TotalFlex ?? 0)
        );
    }

    /// <summary>
    /// Kept for backwards compatibility — prefer GetSkillTotal which includes roll modifiers.
    /// </summary>
    public int CalculateSkillTotal(Ego ego, EgoSkill skill) =>
        GetSkillTotal(ego, skill, GetActiveMorph(ego));

    private static int SumActive(IEnumerable<RollModifier> modifiers, RollModifierType type, string? name = null) =>
        modifiers.Where(x => x.IsActive && x.Type == type && (name == null || x.Name == name))
                 .Sum(x => x.Value);

    #endregion
}
