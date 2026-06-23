using EPTools.Core.Models.Data;
using EPTools.Core.Models.Ego;

namespace EPTools.Core.Interfaces;

public interface IEgoManager
{
    // Skills
    EgoSkill AddKnowledgeSkill(Ego ego, string name = "New Knowledge Skill", string aptitude = "Intuition");
    EgoSkill AddExoticSkill(Ego ego, string name = "New Exotic Skill", string aptitude = "Reflexes");
    bool RemoveSkill(Ego ego, EgoSkill skill);
    bool SetSkillAptitude(Ego ego, EgoSkill skill, string aptitude);

    // Morphs
    Morph ApplyMorphTemplate(Ego ego, MorphTemplate template);
    Morph AddMorph(Ego ego, string name = "New Morph", string morphType = "Biomorph", string size = "Medium");
    bool RemoveMorph(Ego ego, Morph morph);
    void SetActiveMorph(Ego ego, Morph morph);
    Morph? GetActiveMorph(Ego ego);

    // Morph Traits
    EgoTrait AddMorphTrait(Morph morph, string name = "New Trait");
    bool RemoveMorphTrait(Morph morph, EgoTrait trait);

    // Morph Ware
    Ware AddMorphWare(Morph morph, string name = "New Ware", int quantity = 1);
    bool RemoveMorphWare(Morph morph, Ware ware);

    // Psi
    EgoSleight AddSleight(Ego ego, EgoSleight sleight);
    bool RemoveSleight(Ego ego, EgoSleight sleight);
    void AddInfectionEvent(Ego ego, string infectionEvent);
    bool RemoveInfectionEvent(Ego ego, string infectionEvent);
    void SetInfectionRating(Ego ego, int rating);

    // Identities
    Identity AddIdentity(Ego ego, string alias = "New Identity");
    void AddIdentity(Ego ego, Identity identity);
    bool AddReputation(Identity identity, string network, int amount);
    bool RemoveIdentity(Ego ego, Identity identity);

    // Ego Traits
    EgoTrait AddEgoTrait(Ego ego, EgoTrait trait);
    EgoTrait AddEgoTrait(Ego ego, string name = "New Trait");
    bool RemoveEgoTrait(Ego ego, EgoTrait trait);

    // Inventory
    InventoryItem AddInventoryItem(Ego ego, InventoryItem item);
    InventoryItem AddInventoryItem(Ego ego, string name = "New Item", int quantity = 1);
    bool RemoveInventoryItem(Ego ego, InventoryItem item);
    bool MoveItemToCache(Ego ego, InventoryItem item, InventoryCache cache);
    bool MoveItemFromCache(Ego ego, InventoryItem item, InventoryCache cache);

    // Inventory Caches
    InventoryCache AddCache(Ego ego, string location = "New Cache");
    bool RemoveCache(Ego ego, InventoryCache cache);
    InventoryItem AddItemToCache(InventoryCache cache, InventoryItem item);
    InventoryItem AddItemToCache(InventoryCache cache, string name = "New Item", int quantity = 1);
    bool RemoveItemFromCache(InventoryCache cache, InventoryItem item);
    bool AddMorphToCache(Ego ego, Morph morph, InventoryCache cache);
    bool RemoveMorphFromCache(Ego ego, Morph morph, InventoryCache cache);

    // Roll Modifiers
    RollModifier AddEgoModifier(Ego ego, RollModifier modifier);
    bool RemoveEgoModifier(Ego ego, RollModifier modifier);
    void RemoveEgoModifiersBySource(Ego ego, string source);
    RollModifier AddMorphModifier(Morph morph, RollModifier modifier);
    bool RemoveMorphModifier(Morph morph, RollModifier modifier);
    void RemoveMorphModifiersBySource(Morph morph, string source);

    // Calculations
    int GetSkillTotal(Ego ego, EgoSkill skill, Morph? activeMorph = null);
    int GetAptitudeCheckTotal(Ego ego, string aptitudeName, Morph? activeMorph = null);
    int GetInitiative(Ego ego, Morph? activeMorph = null);
    int GetLucidity(Ego ego);
    int GetTraumaThreshold(Ego ego);
    int GetInsanityRating(Ego ego);
    (int Insight, int Moxie, int Vigor, int Flex) GetTotalPools(Ego ego);
}
