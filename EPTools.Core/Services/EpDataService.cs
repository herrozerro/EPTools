using EPTools.Core.Interfaces;
using EPTools.Core.Models;
using EPTools.Core.Models.EPDataModels;
using EPTools.Core.Models.LifePathGen;

namespace EPTools.Core.Services;

public class EpDataService(IFetchService fetchService, IUserDataStore userStore)
{
    private List<Aptitude> _aptitudes = [];
    private List<AptitudeTemplate> _aptitudeTemplates = [];
    private List<Background> _backgrounds = [];
    private List<Career> _careers = [];
    private List<CharGen> _charGen = [];
    private List<Faction> _factions = [];
    private List<GearArmor> _gearArmors = [];
    private List<GearBot> _gearBots = [];
    private List<GearCategories> _gearCategories = [];
    private List<GearComms> _gearComms = [];
    private List<GearCreature> _gearCreatures = [];
    private List<GearDrug> _gearDrugs = [];
    private List<GearItem> _gearItems = [];
    private List<GearMission> _gearMission = [];
    private List<GearNano> _gearNano = [];
    private List<GearPack> _gearPacks = [];
    private List<GearSecurity> _gearSecurity = [];
    private List<GearService> _gearServices = [];
    private List<GearSoftware> _gearSoftware = [];
    private List<GearSwarm> _gearSwarms = [];
    private List<GearVehicle> _gearVehicles = [];
    private List<GearWare> _gearWare = [];
    private List<GearWeaponAmmo> _gearWeaponAmmo = [];
    private List<GearWeaponMelee> _gearWeaponMelees = [];
    private List<GearWeaponRanged> _gearWeaponRanged = [];
    private List<Morph> _morphs = [];
    private List<Skill> _skills = [];
    private List<Sleight> _slights = [];
        

    private List<Trait> _traits = [];
    private List<LifePathNode> _lifepathNativeTongue = [];
    private List<LifePathNode> _lifepathBackgroundPath = [];
    private List<LifePathNode> _lifepathYouthEvents  = [];
    private List<LifePathNode> _lifepathAges = [];
    private List<LifePathNode> _lifepathAdvancedAges = [];
    
    private readonly Dictionary<Type, object> _customDataCache = new();
    private string GetStorageKey<T>() => $"EP_Custom_{typeof(T).Name}";

    public async Task<List<Aptitude>> GetAptitudesAsync()
    {
        if (_aptitudes.Count == 0)
        {
            _aptitudes = await fetchService.GetTFromEpFileAsync<List<Aptitude>>("Aptitudes");
        }
        return _aptitudes;
    }

    public async Task<List<AptitudeTemplate>> GetAptitudeTemplates()
    {
        if (_aptitudeTemplates.Count == 0)
        {
            _aptitudeTemplates = await fetchService.GetTFromEpFileAsync<List<AptitudeTemplate>>("AptitudeTemplates");
        }
        return _aptitudeTemplates;
    }

    public async Task<List<Background>> GetBackgrounds()
    {
        if (_backgrounds.Count == 0)
        {
            _backgrounds = await fetchService.GetTFromEpFileAsync<List<Background>>("Backgrounds");
        }
        return _backgrounds;
    }

    public async Task<List<Career>> GetCareers()
    {
        if (_careers.Count == 0)
        {
            _careers = await fetchService.GetTFromEpFileAsync<List<Career>>("Careers");
        }
        return _careers;
    }

    public async Task<List<CharGen>> GetCharGen()
    {
        if (_charGen.Count == 0)
        {
            _charGen = await fetchService.GetTFromEpFileAsync<List<CharGen>>("CharGen");
        }
        return _charGen;
    }

    public async Task<List<Faction>> GetFactions()
    {
        if (_factions.Count == 0)
        {
            _factions = await fetchService.GetTFromEpFileAsync<List<Faction>>("Factions");
        }
        return _factions;
    }

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
        // 1. Official
        var official = await GetTraitsAsync();

        // 2. Custom
        var custom = await GetCustomListInternalAsync<Trait>();

        // 3. Merge
        return custom.Concat(official).ToList();
    }

    public async Task<List<Morph>> GetAllMorphsAsync()
    {
        var official = await GetMorphsAsync();
        var custom = await GetCustomListInternalAsync<Morph>();
        return custom.Concat(official).ToList();
    }

    public async Task<List<Sleight>> GetAllSleightsAsync()
    {
        var official = await GetSleightsAsync();
        var custom = await GetCustomListInternalAsync<Sleight>();
        return custom.Concat(official).ToList();
    }
        
    public async Task<List<GearArmor>> GetGearArmorsAsync()
    {
        if (_gearArmors.Count == 0)
        {
            _gearArmors = await fetchService.GetTFromEpFileAsync<List<GearArmor>>("GearArmor");
        }
        return _gearArmors;
    }

    public async Task<List<GearBot>> GetGearBotsAsync()
    {
        if (_gearBots.Count == 0)
        {
            _gearBots = await fetchService.GetTFromEpFileAsync<List<GearBot>>("GearBots");
        }
        return _gearBots;
    }

    public async Task<List<GearCategories>> GetGearCategoriesAsync()
    {
        if (_gearCategories.Count == 0)
        {
            _gearCategories = await fetchService.GetTFromEpFileAsync<List<GearCategories>>("GearCategories");
        }
        return _gearCategories;
    }

    public async Task<List<GearComms>> GetGearCommsAsync()
    {
        if (_gearComms.Count == 0)
        {
            _gearComms = await fetchService.GetTFromEpFileAsync<List<GearComms>>("GearComms");
        }
        return _gearComms;
    }

    public async Task<List<GearCreature>> GetGearCreaturesAsync()
    {
        if (_gearCreatures.Count == 0)
        {
            _gearCreatures = await fetchService.GetTFromEpFileAsync<List<GearCreature>>("GearCreatures");
        }
        return _gearCreatures;
    }

    public async Task<List<GearDrug>> GetGearDrugsAsync()
    {
        if (_gearDrugs.Count == 0)
        {
            _gearDrugs = await fetchService.GetTFromEpFileAsync<List<GearDrug>>("GearDrugs");
        }
        return _gearDrugs;
    }

    public async Task<List<GearItem>> GetGearItemsAsync()
    {
        if (_gearItems.Count == 0)
        {
            _gearItems = await fetchService.GetTFromEpFileAsync<List<GearItem>>("GearItems");
        }
        return _gearItems;
    }

    public async Task<List<GearMission>> GetGearMissionAsync()
    {
        if (_gearMission.Count == 0)
        {
            _gearMission = await fetchService.GetTFromEpFileAsync<List<GearMission>>("GearMission");
        }
        return _gearMission;
    }

    public async Task<List<GearNano>> GetGearNanoAsync()
    {
        if (_gearNano.Count == 0)
        {
            _gearNano = await fetchService.GetTFromEpFileAsync<List<GearNano>>("GearNano");
        }
        return _gearNano;
    }

    public async Task<List<GearPack>> GetGearPacksAsync()
    {
        if (_gearPacks.Count == 0)
        {
            _gearPacks = await fetchService.GetTFromEpFileAsync<List<GearPack>>("GearPacks");
        }
        return _gearPacks;
    }

    public async Task<List<GearSecurity>> GetGearSecurityAsync()
    {
        if (_gearSecurity.Count == 0)
        {
            _gearSecurity = await fetchService.GetTFromEpFileAsync<List<GearSecurity>>("GearSecurity");
        }
        return _gearSecurity;
    }
        
    public async Task<List<GearService>> GetGearServicesAsync()
    {
        if (_gearServices.Count == 0)
        {
            _gearServices = await fetchService.GetTFromEpFileAsync<List<GearService>>("GearServices");
        }
        return _gearServices;
    }

    public async Task<List<GearSoftware>> GetGearSoftwareAsync()
    {
        if (_gearSoftware.Count == 0)
        {
            _gearSoftware = await fetchService.GetTFromEpFileAsync<List<GearSoftware>>("GearSoftware");
        }
        return _gearSoftware;
    }

    public async Task<List<GearSwarm>> GetGearSwarmsAsync()
    {
        if (_gearSwarms.Count == 0)
        {
            _gearSwarms = await fetchService.GetTFromEpFileAsync<List<GearSwarm>>("GearSwarms");
        }
        return _gearSwarms;
    }

    public async Task<List<GearVehicle>> GetGearVehiclesAsync()
    {
        if (_gearVehicles.Count == 0)
        {
            _gearVehicles = await fetchService.GetTFromEpFileAsync<List<GearVehicle>>("GearVehicles");
        }
        return _gearVehicles;
    }

    public async Task<List<GearWare>> GetGearWareAsync()
    {
        if (_gearWare.Count == 0)
        {
            _gearWare = await fetchService.GetTFromEpFileAsync<List<GearWare>>("GearWare");
        }
        return _gearWare;
    }

    public async Task<List<Morph>> GetMorphsAsync()
    {
        if (_morphs.Count == 0)
        {
            _morphs = await fetchService.GetTFromEpFileAsync<List<Morph>>("Morphs");
        }
        return _morphs;
    }

    public async Task<List<Trait>> GetTraitsAsync()
    {
        if (_traits.Count == 0)
        {
            _traits = await fetchService.GetTFromEpFileAsync<List<Trait>>("Traits");
        }
        return _traits;
    }

    public async Task<List<LifePathNode>> GetLifepathNativeTonguesAsync()
    {
        if (_lifepathNativeTongue.Count == 0)
        {
            _lifepathNativeTongue = await fetchService.GetTFromEpFileAsync<List<LifePathNode>>("LifePathNativeTongue");
        }
        return _lifepathNativeTongue;
    }

    public async Task<List<LifePathNode>> GetLifepathBackgroundPathAsync()
    {
        if (_lifepathBackgroundPath.Count == 0)
        {
            _lifepathBackgroundPath = await fetchService.GetTFromEpFileAsync<List<LifePathNode>>("LifePathBackgroundPath");
        }
        return _lifepathBackgroundPath;
    }

    public async Task<List<LifePathNode>> GetLifepathYouthEvents()
    {
        if (_lifepathYouthEvents.Count == 0)
        {
            _lifepathYouthEvents = await fetchService.GetTFromEpFileAsync<List<LifePathNode>>("LifePathEventYouth");
        }
        return _lifepathYouthEvents;
    }

    public async Task<List<LifePathNode>> GetLifepathAges()
    {
        if (_lifepathAges.Count == 0)
        {
            _lifepathAges = await fetchService.GetTFromEpFileAsync<List<LifePathNode>>("LifePathAge");
        }
        return _lifepathAges;
    }

    public async Task<List<LifePathNode>> GetLifepathAdvancedAges()
    {
        if (_lifepathAdvancedAges.Count == 0)
        {
            _lifepathAdvancedAges = await fetchService.GetTFromEpFileAsync<List<LifePathNode>>("LifePathAdvancedAge");
        }
        return _lifepathAdvancedAges;
    }

    public async Task<List<LifePathNode>> GetCharacterGenTableAsync(string tableName)
    {
        return await fetchService.GetTFromEpFileAsync<List<LifePathNode>>(tableName);
    }
        
    public async Task<List<Skill>> GetSkillsAsync()
    {
        if (_skills.Count == 0)
        {
            _skills = await fetchService.GetTFromEpFileAsync<List<Skill>>("Skills");
        }
        return _skills;
    }
        
    public async Task<List<Sleight>> GetSleightsAsync()
    {
        if (_slights.Count == 0)
        {
            _slights = await fetchService.GetTFromEpFileAsync<List<Sleight>>("Sleights");
        }
        return _slights;
    }

    public async Task<List<GearWeaponAmmo>> GetGearWeaponAmmoAsync()
    {
        if (_gearWeaponAmmo.Count == 0)
        {
            _gearWeaponAmmo = await fetchService.GetTFromEpFileAsync<List<GearWeaponAmmo>>("GearWeaponAmmo");
        }
        return _gearWeaponAmmo;
    }

    public async Task<List<GearWeaponRanged>> GetGearWeaponRangedAsync()
    {
        if (_gearWeaponRanged.Count == 0)
        {
            _gearWeaponRanged = await fetchService.GetTFromEpFileAsync<List<GearWeaponRanged>>("GearWeaponRanged");
        }
        return _gearWeaponRanged;
    }

    public async Task<List<GearWeaponMelee>> GetGearWeaponMeleeAsync()
    {
        if (_gearWeaponMelees.Count == 0)
        {
            _gearWeaponMelees = await fetchService.GetTFromEpFileAsync<List<GearWeaponMelee>>("GearWeaponMelee");
        }
        return _gearWeaponMelees;
    }
    
    private async Task<List<T>> GetCustomListInternalAsync<T>()
    {
        var type = typeof(T);

        // A. Check Cache
        if (_customDataCache.TryGetValue(type, out var value))
        {
            return (List<T>)value;
        }

        // B. Load from Store
        var key = GetStorageKey<T>();
        var stored = await userStore.GetItemAsync<List<T>>(key);
        
        // Initialize if null (First time user)
        var list = stored ?? [];

        // C. Save to Cache
        _customDataCache[type] = list;
        return list;
    }
    
    public async Task AddCustomTemplateAsync<T>(T item) where T : EpModel
    {
        // 1. Get the list
        var list = await GetCustomListInternalAsync<T>();

        // 2. Add
        list.Add(item);

        // 3. Save (Key is based on Type Name)
        // e.g. "EP_Custom_Gear", "EP_Custom_Trait"
        await userStore.SaveItemAsync(GetStorageKey<T>(), list);
    }

    public async Task RemoveCustomTemplateAsync<T>(T item) where T : EpModel
    {
        var list = await GetCustomListInternalAsync<T>();

        list.Remove(item);
        await userStore.SaveItemAsync(GetStorageKey<T>(), list);
    }
    
    // Optional: Public getter if you just want the custom stuff
    public async Task<List<T>> GetCustomTemplatesAsync<T>()
    {
        return await GetCustomListInternalAsync<T>();
    }
}