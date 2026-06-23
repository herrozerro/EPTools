using System.Text.Json;
using EPTools.Core.Interfaces;
using EPTools.Core.Models;
using EPTools.Core.Models.Data;
using EPTools.Core.Models.Ego;
using EPTools.Core.Services;

namespace EPTools.Core.Tests;

// ---------------------------------------------------------------------------
// Test infrastructure
// ---------------------------------------------------------------------------

/// <summary>
/// Reads files from an absolute directory, mirroring the production FileFetchService.
/// </summary>
file class PathFetchService(string basePath) : IFetchService
{
    public async Task<T> GetTFromEpFileAsync<T>(string filename) where T : new()
    {
        var path = Path.Combine(basePath, filename + ".json");
        if (!File.Exists(path)) return new T();
        await using var stream = File.OpenRead(path);
        return await JsonSerializer.DeserializeAsync<T>(stream) ?? new T();
    }

    // Not needed for EpDataService EP data paths
    public Task<T> GetTFromFileAsync<T>(string filename) where T : new() => Task.FromResult(new T());
}

file class NullUserDataStore : IUserDataStore
{
    public Task SaveItemAsync<T>(string key, T item) => Task.CompletedTask;
    public Task<T?> GetItemAsync<T>(string key) => Task.FromResult<T?>(default);
    public Task DeleteItemAsync(string key) => Task.CompletedTask;
}

// ---------------------------------------------------------------------------
// Tests
// ---------------------------------------------------------------------------

public class EpDataServiceTests
{
    // Resolve the Desktop data directory relative to the test binary output path.
    // Test binary: EPTools.Core.Tests/bin/Debug/net10.0/
    // Up 4 levels: EPTools/ root
    private static readonly string DataDir = Path.GetFullPath(
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..",
            "EPTools.Desktop", "data", "EP-Data"));

    private static EpDataService Make() => new(new PathFetchService(DataDir), new NullUserDataStore());

    // --- Core data files ---

    [Fact]
    public async Task GetSkillsAsync_ReturnsNonEmpty_WithAptitudes()
    {
        var svc = Make();
        var skills = await svc.GetSkillsAsync();

        Assert.NotEmpty(skills);
        Assert.All(skills, s => Assert.False(string.IsNullOrEmpty(s.Aptitude)));
    }

    [Fact]
    public async Task GetMorphsAsync_ReturnsNonEmpty_WithNames()
    {
        var svc = Make();
        var morphs = await svc.GetMorphsAsync();

        Assert.NotEmpty(morphs);
        Assert.All(morphs, m => Assert.False(string.IsNullOrEmpty(m.Name)));
    }

    [Fact]
    public async Task GetTraitsAsync_ReturnsNonEmpty()
    {
        var svc = Make();
        var traits = await svc.GetTraitsAsync();
        Assert.NotEmpty(traits);
    }

    [Fact]
    public async Task GetSleightsAsync_ReturnsNonEmpty()
    {
        var svc = Make();
        var sleights = await svc.GetSleightsAsync();
        Assert.NotEmpty(sleights);
    }

    [Fact]
    public async Task GetBackgrounds_ReturnsNonEmpty()
    {
        var svc = Make();
        var bgs = await svc.GetBackgrounds();
        Assert.NotEmpty(bgs);
    }

    [Fact]
    public async Task GetCareers_ReturnsNonEmpty()
    {
        var svc = Make();
        var careers = await svc.GetCareers();
        Assert.NotEmpty(careers);
    }

    [Fact]
    public async Task GetFactions_ReturnsNonEmpty()
    {
        var svc = Make();
        var factions = await svc.GetFactions();
        Assert.NotEmpty(factions);
    }

    [Fact]
    public async Task GetLifepathNativeTonguesAsync_ReturnsNonEmpty()
    {
        var svc = Make();
        var tongues = await svc.GetLifepathNativeTonguesAsync();
        Assert.NotEmpty(tongues);
    }

    // --- Gear type files ---

    [Fact]
    public async Task GetGearArmorsAsync_ReturnsNonEmpty_WithNameAndCategory()
    {
        var svc = Make();
        var items = await svc.GetGearArmorsAsync();
        Assert.NotEmpty(items);
        Assert.All(items, x => Assert.False(string.IsNullOrEmpty(x.Name)));
        Assert.All(items, x => Assert.False(string.IsNullOrEmpty(x.Category)));
    }

    [Fact]
    public async Task GetGearBotsAsync_ReturnsNonEmpty()
    {
        var items = await Make().GetGearBotsAsync();
        Assert.NotEmpty(items);
        Assert.All(items, x => Assert.False(string.IsNullOrEmpty(x.Name)));
    }

    [Fact]
    public async Task GetGearCommsAsync_ReturnsNonEmpty()
    {
        var items = await Make().GetGearCommsAsync();
        Assert.NotEmpty(items);
        Assert.All(items, x => Assert.False(string.IsNullOrEmpty(x.Name)));
    }

    [Fact]
    public async Task GetGearCreaturesAsync_ReturnsNonEmpty()
    {
        var items = await Make().GetGearCreaturesAsync();
        Assert.NotEmpty(items);
        Assert.All(items, x => Assert.False(string.IsNullOrEmpty(x.Name)));
    }

    [Fact]
    public async Task GetGearDrugsAsync_ReturnsNonEmpty()
    {
        var items = await Make().GetGearDrugsAsync();
        Assert.NotEmpty(items);
        Assert.All(items, x => Assert.False(string.IsNullOrEmpty(x.Name)));
    }

    [Fact]
    public async Task GetGearItemsAsync_ReturnsNonEmpty()
    {
        var items = await Make().GetGearItemsAsync();
        Assert.NotEmpty(items);
        Assert.All(items, x => Assert.False(string.IsNullOrEmpty(x.Name)));
    }

    [Fact]
    public async Task GetGearMissionAsync_ReturnsNonEmpty()
    {
        var items = await Make().GetGearMissionAsync();
        Assert.NotEmpty(items);
        Assert.All(items, x => Assert.False(string.IsNullOrEmpty(x.Name)));
    }

    [Fact]
    public async Task GetGearNanoAsync_ReturnsNonEmpty()
    {
        var items = await Make().GetGearNanoAsync();
        Assert.NotEmpty(items);
        Assert.All(items, x => Assert.False(string.IsNullOrEmpty(x.Name)));
    }

    [Fact]
    public async Task GetGearPacksAsync_ReturnsNonEmpty()
    {
        var items = await Make().GetGearPacksAsync();
        Assert.NotEmpty(items);
        Assert.All(items, x => Assert.False(string.IsNullOrEmpty(x.Name)));
    }

    [Fact]
    public async Task GetGearSecurityAsync_ReturnsNonEmpty()
    {
        var items = await Make().GetGearSecurityAsync();
        Assert.NotEmpty(items);
        Assert.All(items, x => Assert.False(string.IsNullOrEmpty(x.Name)));
    }

    [Fact]
    public async Task GetGearServicesAsync_ReturnsNonEmpty()
    {
        var items = await Make().GetGearServicesAsync();
        Assert.NotEmpty(items);
        Assert.All(items, x => Assert.False(string.IsNullOrEmpty(x.Name)));
    }

    [Fact]
    public async Task GetGearSoftwareAsync_ReturnsNonEmpty()
    {
        var items = await Make().GetGearSoftwareAsync();
        Assert.NotEmpty(items);
        Assert.All(items, x => Assert.False(string.IsNullOrEmpty(x.Name)));
    }

    [Fact]
    public async Task GetGearSwarmsAsync_ReturnsNonEmpty()
    {
        var items = await Make().GetGearSwarmsAsync();
        Assert.NotEmpty(items);
        Assert.All(items, x => Assert.False(string.IsNullOrEmpty(x.Name)));
    }

    [Fact]
    public async Task GetGearVehiclesAsync_ReturnsNonEmpty()
    {
        var items = await Make().GetGearVehiclesAsync();
        Assert.NotEmpty(items);
        Assert.All(items, x => Assert.False(string.IsNullOrEmpty(x.Name)));
    }

    [Fact]
    public async Task GetGearWareAsync_ReturnsNonEmpty()
    {
        var items = await Make().GetGearWareAsync();
        Assert.NotEmpty(items);
        Assert.All(items, x => Assert.False(string.IsNullOrEmpty(x.Name)));
    }

    [Fact]
    public async Task GetGearWeaponAmmoAsync_ReturnsNonEmpty()
    {
        var items = await Make().GetGearWeaponAmmoAsync();
        Assert.NotEmpty(items);
        Assert.All(items, x => Assert.False(string.IsNullOrEmpty(x.Name)));
    }

    [Fact]
    public async Task GetGearWeaponMeleeAsync_ReturnsNonEmpty()
    {
        var items = await Make().GetGearWeaponMeleeAsync();
        Assert.NotEmpty(items);
        Assert.All(items, x => Assert.False(string.IsNullOrEmpty(x.Name)));
    }

    [Fact]
    public async Task GetGearWeaponRangedAsync_ReturnsNonEmpty()
    {
        var items = await Make().GetGearWeaponRangedAsync();
        Assert.NotEmpty(items);
        Assert.All(items, x => Assert.False(string.IsNullOrEmpty(x.Name)));
    }

    // --- Aggregate ---

    [Fact]
    public async Task GetAllGearAsync_ContainsAllSeventeenConcreteTypes()
    {
        var svc = Make();
        var all = await svc.GetAllGearAsync();

        Assert.NotEmpty(all);
        Assert.Contains(all, g => g is GearArmor);
        Assert.Contains(all, g => g is GearBot);
        Assert.Contains(all, g => g is GearComms);
        Assert.Contains(all, g => g is GearCreature);
        Assert.Contains(all, g => g is GearDrug);
        Assert.Contains(all, g => g is GearItem);
        Assert.Contains(all, g => g is GearMission);
        Assert.Contains(all, g => g is GearNano);
        Assert.Contains(all, g => g is GearSecurity);
        Assert.Contains(all, g => g is GearService);
        Assert.Contains(all, g => g is GearSoftware);
        Assert.Contains(all, g => g is GearSwarm);
        Assert.Contains(all, g => g is GearVehicle);
        Assert.Contains(all, g => g is GearWare);
        Assert.Contains(all, g => g is GearWeaponAmmo);
        Assert.Contains(all, g => g is GearWeaponMelee);
        Assert.Contains(all, g => g is GearWeaponRanged);
    }

    [Fact]
    public async Task GetAllGearAsync_AllItemsHaveNonEmptyName()
    {
        var svc = Make();
        var all = await svc.GetAllGearAsync();
        Assert.All(all, g => Assert.False(string.IsNullOrEmpty(g.Name)));
    }

    // --- Caching ---

    [Fact]
    public async Task GetSkillsAsync_SecondCall_ReturnsSameListInstance()
    {
        var svc = Make();
        var first = await svc.GetSkillsAsync();
        var second = await svc.GetSkillsAsync();
        Assert.Same(first, second);
    }

    // --- Polymorphic round-trip (InventoryItem.BaseGear) ---
    // Each concrete Gear subtype must survive: Gear? → JSON (with $type) → Gear? deserialization.

    public static TheoryData<Type, Gear> GearRoundTripCases { get; } = BuildRoundTripCases();

    private static TheoryData<Type, Gear> BuildRoundTripCases()
    {
        // Build by loading real first items from each file
        var dir = Path.GetFullPath(
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..",
                "EPTools.Desktop", "data", "EP-Data"));

        static T? Load<T>(string dir, string file) where T : class
        {
            var path = Path.Combine(dir, file + ".json");
            if (!File.Exists(path)) return null;
            using var stream = File.OpenRead(path);
            var list = JsonSerializer.Deserialize<List<T>>(stream);
            return list?.FirstOrDefault();
        }

        var cases = new TheoryData<Type, Gear>();
        void Add<T>(string file) where T : Gear
        {
            var item = Load<T>(dir, file);
            if (item != null) cases.Add(typeof(T), item);
        }

        Add<GearArmor>("GearArmor");
        Add<GearBot>("GearBots");
        Add<GearComms>("GearComms");
        Add<GearCreature>("GearCreatures");
        Add<GearDrug>("GearDrugs");
        Add<GearItem>("GearItems");
        Add<GearMission>("GearMission");
        Add<GearNano>("GearNano");
        Add<GearSecurity>("GearSecurity");
        Add<GearService>("GearServices");
        Add<GearSoftware>("GearSoftware");
        Add<GearSwarm>("GearSwarms");
        Add<GearVehicle>("GearVehicles");
        Add<GearWare>("GearWare");
        Add<GearWeaponAmmo>("GearWeaponAmmo");
        Add<GearWeaponMelee>("GearWeaponMelee");
        Add<GearWeaponRanged>("GearWeaponRanged");

        return cases;
    }

    [Theory]
    [MemberData(nameof(GearRoundTripCases))]
    public void InventoryItem_BaseGear_RoundTripsCorrectConcreteType(Type expectedType, Gear gear)
    {
        var item = new InventoryItem { Name = gear.Name, BaseGear = gear };

        var json = JsonSerializer.Serialize(item);
        var restored = JsonSerializer.Deserialize<InventoryItem>(json);

        Assert.NotNull(restored?.BaseGear);
        Assert.Equal(expectedType, restored!.BaseGear!.GetType());
        Assert.Equal(gear.Name, restored.BaseGear.Name);
    }
}
