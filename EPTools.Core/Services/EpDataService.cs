using EPTools.Core.Interfaces;
using EPTools.Core.Models;
using EPTools.Core.Models.EPDataModels;
using EPTools.Core.Models.LifePathGen;

namespace EPTools.Core.Services;

public class EpDataService(IFetchService fetchService, IUserDataStore userStore) : IEpDataService
{
    private readonly Dictionary<string, object> _cache = new();
    private readonly Dictionary<Type, object> _customDataCache = new();

    private async Task<List<T>> GetCachedAsync<T>(string filename)
    {
        if (_cache.TryGetValue(filename, out var cached))
            return (List<T>)cached;

        var data = await fetchService.GetTFromEpFileAsync<List<T>>(filename);
        _cache[filename] = data;
        return data;
    }

    private string GetStorageKey<T>() => $"EP_Custom_{typeof(T).Name}";

    // EP Data Accessors
    public Task<List<Aptitude>> GetAptitudesAsync() => GetCachedAsync<Aptitude>("Aptitudes");
    public Task<List<AptitudeTemplate>> GetAptitudeTemplates() => GetCachedAsync<AptitudeTemplate>("AptitudeTemplates");
    public Task<List<Background>> GetBackgrounds() => GetCachedAsync<Background>("Backgrounds");
    public Task<List<Career>> GetCareers() => GetCachedAsync<Career>("Careers");
    public Task<List<CharGen>> GetCharGen() => GetCachedAsync<CharGen>("CharGen");
    public Task<List<Faction>> GetFactions() => GetCachedAsync<Faction>("Factions");
    public Task<List<GearArmor>> GetGearArmorsAsync() => GetCachedAsync<GearArmor>("GearArmor");
    public Task<List<GearBot>> GetGearBotsAsync() => GetCachedAsync<GearBot>("GearBots");
    public Task<List<GearCategories>> GetGearCategoriesAsync() => GetCachedAsync<GearCategories>("GearCategories");
    public Task<List<GearComms>> GetGearCommsAsync() => GetCachedAsync<GearComms>("GearComms");
    public Task<List<GearCreature>> GetGearCreaturesAsync() => GetCachedAsync<GearCreature>("GearCreatures");
    public Task<List<GearDrug>> GetGearDrugsAsync() => GetCachedAsync<GearDrug>("GearDrugs");
    public Task<List<GearItem>> GetGearItemsAsync() => GetCachedAsync<GearItem>("GearItems");
    public Task<List<GearMission>> GetGearMissionAsync() => GetCachedAsync<GearMission>("GearMission");
    public Task<List<GearNano>> GetGearNanoAsync() => GetCachedAsync<GearNano>("GearNano");
    public Task<List<GearPack>> GetGearPacksAsync() => GetCachedAsync<GearPack>("GearPacks");
    public Task<List<GearSecurity>> GetGearSecurityAsync() => GetCachedAsync<GearSecurity>("GearSecurity");
    public Task<List<GearService>> GetGearServicesAsync() => GetCachedAsync<GearService>("GearServices");
    public Task<List<GearSoftware>> GetGearSoftwareAsync() => GetCachedAsync<GearSoftware>("GearSoftware");
    public Task<List<GearSwarm>> GetGearSwarmsAsync() => GetCachedAsync<GearSwarm>("GearSwarms");
    public Task<List<GearVehicle>> GetGearVehiclesAsync() => GetCachedAsync<GearVehicle>("GearVehicles");
    public Task<List<GearWare>> GetGearWareAsync() => GetCachedAsync<GearWare>("GearWare");
    public Task<List<GearWeaponAmmo>> GetGearWeaponAmmoAsync() => GetCachedAsync<GearWeaponAmmo>("GearWeaponAmmo");
    public Task<List<GearWeaponMelee>> GetGearWeaponMeleeAsync() => GetCachedAsync<GearWeaponMelee>("GearWeaponMelee");
    public Task<List<GearWeaponRanged>> GetGearWeaponRangedAsync() => GetCachedAsync<GearWeaponRanged>("GearWeaponRanged");
    public Task<List<MorphTemplate>> GetMorphsAsync() => GetCachedAsync<MorphTemplate>("Morphs");
    public Task<List<Skill>> GetSkillsAsync() => GetCachedAsync<Skill>("Skills");
    public Task<List<Sleight>> GetSleightsAsync() => GetCachedAsync<Sleight>("Sleights");
    public Task<List<Trait>> GetTraitsAsync() => GetCachedAsync<Trait>("Traits");

    // LifePath Data Accessors
    public Task<List<LifePathNode>> GetLifepathNativeTonguesAsync() => GetCachedAsync<LifePathNode>("LifePathNativeTongue");
    public Task<List<LifePathNode>> GetLifepathBackgroundPathAsync() => GetCachedAsync<LifePathNode>("LifePathBackgroundPath");
    public Task<List<LifePathNode>> GetLifepathYouthEvents() => GetCachedAsync<LifePathNode>("LifePathEventYouth");
    public Task<List<LifePathNode>> GetLifepathAges() => GetCachedAsync<LifePathNode>("LifePathAge");
    public Task<List<LifePathNode>> GetLifepathAdvancedAges() => GetCachedAsync<LifePathNode>("LifePathAdvancedAge");

    public async Task<List<LifePathNode>> GetCharacterGenTableAsync(string tableName)
    {
        return await fetchService.GetTFromEpFileAsync<List<LifePathNode>>(tableName);
    }

    // Aggregate Accessors (official + custom)
    public async Task<List<Gear>> GetAllGearAsync()
    {
        var officialGear = new List<Gear>();
        officialGear.AddRange(await GetGearArmorsAsync());
        officialGear.AddRange(await GetGearBotsAsync());
        officialGear.AddRange(await GetGearCommsAsync());
        officialGear.AddRange(await GetGearCreaturesAsync());
        officialGear.AddRange(await GetGearDrugsAsync());
        officialGear.AddRange(await GetGearItemsAsync());
        officialGear.AddRange(await GetGearMissionAsync());
        officialGear.AddRange(await GetGearNanoAsync());
        officialGear.AddRange(await GetGearSecurityAsync());
        officialGear.AddRange(await GetGearSoftwareAsync());
        officialGear.AddRange(await GetGearSwarmsAsync());
        officialGear.AddRange(await GetGearVehiclesAsync());
        officialGear.AddRange(await GetGearWareAsync());
        officialGear.AddRange(await GetGearWeaponAmmoAsync());
        officialGear.AddRange(await GetGearWeaponMeleeAsync());
        officialGear.AddRange(await GetGearWeaponRangedAsync());
        officialGear.AddRange(await GetGearServicesAsync());

        var customGear = await GetCustomListInternalAsync<Gear>();

        return customGear.Concat(officialGear).ToList();
    }

    public async Task<List<Trait>> GetAllTraitsAsync()
    {
        var official = await GetTraitsAsync();
        var custom = await GetCustomListInternalAsync<Trait>();
        return custom.Concat(official).ToList();
    }

    public async Task<List<MorphTemplate>> GetAllMorphsAsync()
    {
        var official = await GetMorphsAsync();
        var custom = await GetCustomListInternalAsync<MorphTemplate>();
        return custom.Concat(official).ToList();
    }

    public async Task<List<Sleight>> GetAllSleightsAsync()
    {
        var official = await GetSleightsAsync();
        var custom = await GetCustomListInternalAsync<Sleight>();
        return custom.Concat(official).ToList();
    }

    // Custom Data Management
    private async Task<List<T>> GetCustomListInternalAsync<T>()
    {
        var type = typeof(T);

        if (_customDataCache.TryGetValue(type, out var value))
        {
            return (List<T>)value;
        }

        var key = GetStorageKey<T>();
        var stored = await userStore.GetItemAsync<List<T>>(key);
        var list = stored ?? [];

        _customDataCache[type] = list;
        return list;
    }

    public async Task AddCustomTemplateAsync<T>(T item) where T : EpModel
    {
        var list = await GetCustomListInternalAsync<T>();
        list.Add(item);
        await userStore.SaveItemAsync(GetStorageKey<T>(), list);
    }

    public async Task RemoveCustomTemplateAsync<T>(T item) where T : EpModel
    {
        var list = await GetCustomListInternalAsync<T>();
        list.Remove(item);
        await userStore.SaveItemAsync(GetStorageKey<T>(), list);
    }

    public async Task<List<T>> GetCustomTemplatesAsync<T>()
    {
        return await GetCustomListInternalAsync<T>();
    }
}
