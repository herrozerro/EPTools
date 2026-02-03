using EPTools.Core.Constants;
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
    /// Adds a trait to the ego.
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
    /// Adds an item to the ego's local inventory.
    /// </summary>
    public InventoryItem AddInventoryItem(Ego ego, string name = "New Item", int quantity = 1)
    {
        var item = new InventoryItem
        {
            Name = name,
            Quantity = quantity
        };
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
    /// Moves an item from local inventory to a cache.
    /// </summary>
    public bool MoveItemToCache(Ego ego, InventoryItem item, InventoryCache cache)
    {
        if (!ego.Inventory.Remove(item))
            return false;

        cache.Inventory.Add(item);
        return true;
    }

    /// <summary>
    /// Moves an item from a cache to local inventory.
    /// </summary>
    public bool MoveItemFromCache(Ego ego, InventoryItem item, InventoryCache cache)
    {
        if (!cache.Inventory.Remove(item))
            return false;

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
    /// Adds an item directly to a cache.
    /// </summary>
    public InventoryItem AddItemToCache(InventoryCache cache, string name = "New Item", int quantity = 1)
    {
        var item = new InventoryItem
        {
            Name = name,
            Quantity = quantity
        };
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

    #region Calculations

    /// <summary>
    /// Calculates the total value for a skill (rank + linked aptitude).
    /// </summary>
    public int CalculateSkillTotal(Ego ego, EgoSkill skill)
    {
        return ego.SkillTotal(skill);
    }

    /// <summary>
    /// Gets the total pools combining ego flex and active morph pools.
    /// </summary>
    public (int Insight, int Moxie, int Vigor, int Flex) GetTotalPools(Ego ego)
    {
        var activeMorph = GetActiveMorph(ego);

        return (
            Insight: activeMorph?.Insight ?? 0,
            Moxie: activeMorph?.Moxie ?? 0,
            Vigor: activeMorph?.Vigor ?? 0,
            Flex: ego.EgoFlex + (activeMorph?.MorphFlex ?? 0)
        );
    }

    #endregion
}
