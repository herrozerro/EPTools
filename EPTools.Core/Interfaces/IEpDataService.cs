using EPTools.Core.Models;
using EPTools.Core.Models.EPDataModels;
using EPTools.Core.Models.LifePathGen;

namespace EPTools.Core.Interfaces;

public interface IEpDataService
{
    Task<List<Aptitude>> GetAptitudesAsync();
    Task<List<AptitudeTemplate>> GetAptitudeTemplates();
    Task<List<Background>> GetBackgrounds();
    Task<List<Career>> GetCareers();
    Task<List<CharGen>> GetCharGen();
    Task<List<Faction>> GetFactions();
    Task<List<GearArmor>> GetGearArmorsAsync();
    Task<List<GearBot>> GetGearBotsAsync();
    Task<List<GearCategories>> GetGearCategoriesAsync();
    Task<List<GearComms>> GetGearCommsAsync();
    Task<List<GearCreature>> GetGearCreaturesAsync();
    Task<List<GearDrug>> GetGearDrugsAsync();
    Task<List<GearItem>> GetGearItemsAsync();
    Task<List<GearMission>> GetGearMissionAsync();
    Task<List<GearNano>> GetGearNanoAsync();
    Task<List<GearPack>> GetGearPacksAsync();
    Task<List<GearSecurity>> GetGearSecurityAsync();
    Task<List<GearService>> GetGearServicesAsync();
    Task<List<GearSoftware>> GetGearSoftwareAsync();
    Task<List<GearSwarm>> GetGearSwarmsAsync();
    Task<List<GearVehicle>> GetGearVehiclesAsync();
    Task<List<GearWare>> GetGearWareAsync();
    Task<List<GearWeaponAmmo>> GetGearWeaponAmmoAsync();
    Task<List<GearWeaponMelee>> GetGearWeaponMeleeAsync();
    Task<List<GearWeaponRanged>> GetGearWeaponRangedAsync();
    Task<List<MorphTemplate>> GetMorphsAsync();
    Task<List<Skill>> GetSkillsAsync();
    Task<List<Sleight>> GetSleightsAsync();
    Task<List<Trait>> GetTraitsAsync();

    Task<List<LifePathNode>> GetLifepathNativeTonguesAsync();
    Task<List<LifePathNode>> GetLifepathBackgroundPathAsync();
    Task<List<LifePathNode>> GetLifepathYouthEvents();
    Task<List<LifePathNode>> GetLifepathAges();
    Task<List<LifePathNode>> GetLifepathAdvancedAges();
    Task<List<LifePathNode>> GetCharacterGenTableAsync(string tableName);

    Task<List<Gear>> GetAllGearAsync();
    Task<List<Trait>> GetAllTraitsAsync();
    Task<List<MorphTemplate>> GetAllMorphsAsync();
    Task<List<Sleight>> GetAllSleightsAsync();

    Task AddCustomTemplateAsync<T>(T item) where T : EpModel;
    Task RemoveCustomTemplateAsync<T>(T item) where T : EpModel;
    Task<List<T>> GetCustomTemplatesAsync<T>();
}
