using EPTools.Core.Interfaces;
using EPTools.Core.Models.EPDataModels;
using EPTools.Core.Models.LifePathGen;

namespace EPTools.Core.Services
{
    public class EpDataService(IFetchService fetchService)
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
        private List<GearSoftware> _gearSoftware = [];
        private List<GearSwarm> _gearSwarms = [];
        private List<GearVehicle> _gearVehicles = [];
        private List<GearWare> _gearWare = [];
        private List<GearWeaponAmmo> _gearWeaponAmmo = [];
        private List<GearWeaponMelee> _gearWeaponMelees = [];
        private List<GearWeaponRanged> _gearWeaponRanged = [];
        private List<Morph> _morphs = [];
        private List<Skill> _skills = [];
        private List<Slight> _slights = [];
        

        private List<Trait> _traits = [];
        private List<LifePathNode> _lifepathNativeTongue = [];
        private List<LifePathNode> _lifepathBackgroundPath = [];
        private List<LifePathNode> _lifepathYouthEvents  = [];
        private List<LifePathNode> _lifepathAges = [];
        private List<LifePathNode> _lifepathAdvancedAges = [];

        public async Task<List<Aptitude>> GetAptitudes()
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

        public async Task<List<GearArmor>> GetGearArmors()
        {
            if (_gearArmors.Count == 0)
            {
                _gearArmors = await fetchService.GetTFromEpFileAsync<List<GearArmor>>("GearArmor");
            }
            return _gearArmors;
        }

        public async Task<List<GearBot>> GetGearBots()
        {
            if (_gearBots.Count == 0)
            {
                _gearBots = await fetchService.GetTFromEpFileAsync<List<GearBot>>("GearBots");
            }
            return _gearBots;
        }

        public async Task<List<GearCategories>> GetGearCategories()
        {
            if (_gearCategories.Count == 0)
            {
                _gearCategories = await fetchService.GetTFromEpFileAsync<List<GearCategories>>("GearCategories");
            }
            return _gearCategories;
        }

        public async Task<List<GearComms>> GetGearComms()
        {
            if (_gearComms.Count == 0)
            {
                _gearComms = await fetchService.GetTFromEpFileAsync<List<GearComms>>("GearComms");
            }
            return _gearComms;
        }

        public async Task<List<GearCreature>> GetGearCreatures()
        {
            if (_gearCreatures.Count == 0)
            {
                _gearCreatures = await fetchService.GetTFromEpFileAsync<List<GearCreature>>("GearCreatures");
            }
            return _gearCreatures;
        }

        public async Task<List<GearDrug>> GetGearDrugs()
        {
            if (_gearDrugs.Count == 0)
            {
                _gearDrugs = await fetchService.GetTFromEpFileAsync<List<GearDrug>>("GearDrugs");
            }
            return _gearDrugs;
        }

        public async Task<List<GearItem>> GetGearItems()
        {
            if (_gearItems.Count == 0)
            {
                _gearItems = await fetchService.GetTFromEpFileAsync<List<GearItem>>("GearItems");
            }
            return _gearItems;
        }

        public async Task<List<GearMission>> GetGearMission()
        {
            if (_gearMission.Count == 0)
            {
                _gearMission = await fetchService.GetTFromEpFileAsync<List<GearMission>>("GearMission");
            }
            return _gearMission;
        }

        public async Task<List<GearNano>> GetGearNano()
        {
            if (_gearNano.Count == 0)
            {
                _gearNano = await fetchService.GetTFromEpFileAsync<List<GearNano>>("GearNano");
            }
            return _gearNano;
        }

        public async Task<List<GearPack>> GetGearPacks()
        {
            if (_gearPacks.Count == 0)
            {
                _gearPacks = await fetchService.GetTFromEpFileAsync<List<GearPack>>("GearPacks");
            }
            return _gearPacks;
        }

        public async Task<List<GearSecurity>> GetGearSecurity()
        {
            if (_gearSecurity.Count == 0)
            {
                _gearSecurity = await fetchService.GetTFromEpFileAsync<List<GearSecurity>>("GearSecurity");
            }
            return _gearSecurity;
        }

        public async Task<List<GearSoftware>> GetGearSoftware()
        {
            if (_gearSoftware.Count == 0)
            {
                _gearSoftware = await fetchService.GetTFromEpFileAsync<List<GearSoftware>>("GearSoftware");
            }
            return _gearSoftware;
        }

        public async Task<List<GearSwarm>> GetGearSwarms()
        {
            if (_gearSwarms.Count == 0)
            {
                _gearSwarms = await fetchService.GetTFromEpFileAsync<List<GearSwarm>>("GearSwarms");
            }
            return _gearSwarms;
        }

        public async Task<List<GearVehicle>> GetGearVehicles()
        {
            if (_gearVehicles.Count == 0)
            {
                _gearVehicles = await fetchService.GetTFromEpFileAsync<List<GearVehicle>>("GearVehicles");
            }
            return _gearVehicles;
        }

        public async Task<List<GearWare>> GetGearWare()
        {
            if (_gearWare.Count == 0)
            {
                _gearWare = await fetchService.GetTFromEpFileAsync<List<GearWare>>("GearWare");
            }
            return _gearWare;
        }

        public async Task<List<Morph>> GetMorphs()
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

        public async Task<List<LifePathNode>> GetCharacterGenTable(string tableName)
        {
            return await fetchService.GetTFromEpFileAsync<List<LifePathNode>>(tableName);
        }
        
        public async Task<List<Skill>> GetSkills()
        {
            if (_skills.Count == 0)
            {
                _skills = await fetchService.GetTFromEpFileAsync<List<Skill>>("Skills");
            }
            return _skills;
        }
        
        public async Task<List<Slight>> GetSlights()
        {
            if (_slights.Count == 0)
            {
                _slights = await fetchService.GetTFromEpFileAsync<List<Slight>>("Slights");
            }
            return _slights;
        }

        public async Task<List<GearWeaponAmmo>> GetGearWeaponAmmo()
        {
            if (_gearWeaponAmmo.Count == 0)
            {
                _gearWeaponAmmo = await fetchService.GetTFromEpFileAsync<List<GearWeaponAmmo>>("GearWeaponAmmo");
            }
            return _gearWeaponAmmo;
        }

        public async Task<List<GearWeaponRanged>> GetGearWeaponRanged()
        {
            if (_gearWeaponRanged.Count == 0)
            {
                _gearWeaponRanged = await fetchService.GetTFromEpFileAsync<List<GearWeaponRanged>>("GearWeaponRanged");
            }
            return _gearWeaponRanged;
        }

        public async Task<List<GearWeaponMelee>> GetGearWeaponMelee()
        {
            if (_gearWeaponMelees.Count == 0)
            {
                _gearWeaponMelees = await fetchService.GetTFromEpFileAsync<List<GearWeaponMelee>>("GearWeaponMelee");
            }
            return _gearWeaponMelees;
        }
    }
}
