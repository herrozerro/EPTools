using EPTools.Core.Interfaces;
using EPTools.Core.Models.EPDataModels;
using EPTools.Core.Models.LifePathGen;

namespace EPTools.Core.Services
{
    public class EPDataService(IFetchService fetchService)
    {
        private List<Aptitude> _aptitudes = new();
        private List<AptitudeTemplate> _aptitude_templates = new();
        private List<Background> _backgrounds = new();
        private List<Career> _careers = new();
        private List<CharGen> _chargen = new();
        private List<Faction> _factions = new();
        private List<GearArmor> _gear_armors = new();
        private List<GearBot> _gear_bots = new();
        private List<GearCategories> _gear_categories = new();
        private List<GearComms> _gear_comms = new();
        private List<GearCreature> _gear_creatures = new();
        private List<GearDrug> _gear_drugs = new();
        private List<GearItem> _gear_items = new();
        private List<GearMission> _gear_mission = new();
        private List<GearNano> _gear_nano = new();
        private List<GearPack> _gear_packs = new();
        private List<GearSecurity> _gear_security = new();
        private List<GearSoftware> _gear_software = new();
        private List<GearSwarm> _gear_swarms = new();
        private List<GearVehicle> _gear_vehicles = new();
        private List<GearWare> _gear_ware = new();
        private List<Morph> _morphs = new();
        private List<Skill> _skills = new();

        private List<Trait> _traits = new();
        private List<LifePathNode> _lifepathNativeTongue = new();
        private List<LifePathNode> _lifepathBackgroundPath = new();
        private List<LifePathNode> _lifepathYouthEvents  = new();
        private List<LifePathNode> _lifepathAges = new();
        private List<LifePathNode> _lifepathAdvancedAges = new();

        public async Task<List<Aptitude>> GetAptitudes()
        {
            if (_aptitudes.Count == 0)
            {
                _aptitudes = await fetchService.GetTFromEpFileAsync<List<Aptitude>>("aptitudes");
            }
            return _aptitudes;
        }

        public async Task<List<AptitudeTemplate>> GetAptitudeTemplates()
        {
            if (_aptitude_templates.Count == 0)
            {
                _aptitude_templates = await fetchService.GetTFromEpFileAsync<List<AptitudeTemplate>>("aptitudetemplates");
            }
            return _aptitude_templates;
        }

        public async Task<List<Background>> GetBackgrounds()
        {
            if (_backgrounds.Count == 0)
            {
                _backgrounds = await fetchService.GetTFromEpFileAsync<List<Background>>("backgrounds");
            }
            return _backgrounds;
        }

        public async Task<List<Career>> GetCareers()
        {
            if (_careers.Count == 0)
            {
                _careers = await fetchService.GetTFromEpFileAsync<List<Career>>("careers");
            }
            return _careers;
        }

        public async Task<List<CharGen>> GetCharGen()
        {
            if (_chargen.Count == 0)
            {
                _chargen = await fetchService.GetTFromEpFileAsync<List<CharGen>>("chargen");
            }
            return _chargen;
        }

        public async Task<List<Faction>> GetFactions()
        {
            if (_factions.Count == 0)
            {
                _factions = await fetchService.GetTFromEpFileAsync<List<Faction>>("factions");
            }
            return _factions;
        }

        public async Task<List<GearArmor>> GetGearArmors()
        {
            if (_gear_armors.Count == 0)
            {
                _gear_armors = await fetchService.GetTFromEpFileAsync<List<GearArmor>>("gear_armor");
            }
            return _gear_armors;
        }

        public async Task<List<GearBot>> GetGearBots()
        {
            if (_gear_bots.Count == 0)
            {
                _gear_bots = await fetchService.GetTFromEpFileAsync<List<GearBot>>("gear_bots");
            }
            return _gear_bots;
        }

        public async Task<List<GearCategories>> GetGearCategories()
        {
            if (_gear_categories.Count == 0)
            {
                _gear_categories = await fetchService.GetTFromEpFileAsync<List<GearCategories>>("gear_categories");
            }
            return _gear_categories;
        }

        public async Task<List<GearComms>> GetGearComms()
        {
            if (_gear_comms.Count == 0)
            {
                _gear_comms = await fetchService.GetTFromEpFileAsync<List<GearComms>>("gear_comms");
            }
            return _gear_comms;
        }

        public async Task<List<GearCreature>> GetGearCreatures()
        {
            if (_gear_creatures.Count == 0)
            {
                _gear_creatures = await fetchService.GetTFromEpFileAsync<List<GearCreature>>("gear_creatures");
            }
            return _gear_creatures;
        }

        public async Task<List<GearDrug>> GetGearDrugs()
        {
            if (_gear_drugs.Count == 0)
            {
                _gear_drugs = await fetchService.GetTFromEpFileAsync<List<GearDrug>>("gear_drugs");
            }
            return _gear_drugs;
        }

        public async Task<List<GearItem>> GetGearItems()
        {
            if (_gear_items.Count == 0)
            {
                _gear_items = await fetchService.GetTFromEpFileAsync<List<GearItem>>("gear_items");
            }
            return _gear_items;
        }

        public async Task<List<GearMission>> GetGearMission()
        {
            if (_gear_mission.Count == 0)
            {
                _gear_mission = await fetchService.GetTFromEpFileAsync<List<GearMission>>("gear_mission");
            }
            return _gear_mission;
        }

        public async Task<List<GearNano>> GetGearNano()
        {
            if (_gear_nano.Count == 0)
            {
                _gear_nano = await fetchService.GetTFromEpFileAsync<List<GearNano>>("gear_nano");
            }
            return _gear_nano;
        }

        public async Task<List<GearPack>> GetGearPacks()
        {
            if (_gear_packs.Count == 0)
            {
                _gear_packs = await fetchService.GetTFromEpFileAsync<List<GearPack>>("gear_packs");
            }
            return _gear_packs;
        }

        public async Task<List<GearSecurity>> GetGearSecurity()
        {
            if (_gear_security.Count == 0)
            {
                _gear_security = await fetchService.GetTFromEpFileAsync<List<GearSecurity>>("gear_security");
            }
            return _gear_security;
        }

        public async Task<List<GearSoftware>> GetGearSoftware()
        {
            if (_gear_software.Count == 0)
            {
                _gear_software = await fetchService.GetTFromEpFileAsync<List<GearSoftware>>("gear_software");
            }
            return _gear_software;
        }

        public async Task<List<GearSwarm>> GetGearSwarms()
        {
            if (_gear_swarms.Count == 0)
            {
                _gear_swarms = await fetchService.GetTFromEpFileAsync<List<GearSwarm>>("gear_swarms");
            }
            return _gear_swarms;
        }

        public async Task<List<GearVehicle>> GetGearVehicles()
        {
            if (_gear_vehicles.Count == 0)
            {
                _gear_vehicles = await fetchService.GetTFromEpFileAsync<List<GearVehicle>>("gear_vehicles");
            }
            return _gear_vehicles;
        }

        public async Task<List<GearWare>> GetGearWare()
        {
            if (_gear_ware.Count == 0)
            {
                _gear_ware = await fetchService.GetTFromEpFileAsync<List<GearWare>>("gear_ware");
            }
            return _gear_ware;
        }

        public async Task<List<Morph>> GetMorphs()
        {
            if (_morphs.Count == 0)
            {
                _morphs = await fetchService.GetTFromEpFileAsync<List<Morph>>("morphs");
            }
            return _morphs;
        }

        public async Task<List<Trait>> GetTraitsAsync()
        {
            if (_traits.Count == 0)
            {
                _traits = await fetchService.GetTFromEpFileAsync<List<Trait>>("traits");
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
                _skills = await fetchService.GetTFromEpFileAsync<List<Skill>>("skills");
            }
            return _skills;
        }
    }
}
