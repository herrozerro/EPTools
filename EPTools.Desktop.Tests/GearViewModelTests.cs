using EPTools.Desktop.ViewModels;
using EPTools.Core.Models.Ego;
using EPTools.Core.Services;

namespace EPTools.Desktop.Tests;

public class InventoryItemViewModelTests
{
    [Fact]
    public void Constructor_SetsPropertiesFromModel()
    {
        var item = new InventoryItem
        {
            Name = "Firearm",
            Description = "A pistol",
            Quantity = 1,
            Equipped = true,
            Active = false,
            Notes = "Personal sidearm"
        };

        var vm = new InventoryItemViewModel(item);

        Assert.Equal("Firearm", vm.Name);
        Assert.Equal("A pistol", vm.Description);
        Assert.Equal(1, vm.Quantity);
        Assert.True(vm.Equipped);
        Assert.False(vm.Active);
        Assert.Equal("Personal sidearm", vm.Notes);
    }

    [Fact]
    public void Properties_UpdateModel()
    {
        var item = new InventoryItem();
        var vm = new InventoryItemViewModel(item);

        vm.Name = "Updated Name";
        vm.Description = "Updated Description";
        vm.Quantity = 5;
        vm.Equipped = true;
        vm.Active = true;
        vm.Notes = "Updated Notes";

        Assert.Equal("Updated Name", item.Name);
        Assert.Equal("Updated Description", item.Description);
        Assert.Equal(5, item.Quantity);
        Assert.True(item.Equipped);
        Assert.True(item.Active);
        Assert.Equal("Updated Notes", item.Notes);
    }

    [Fact]
    public void Model_ReturnsUnderlyingItem()
    {
        var item = new InventoryItem { Name = "Test" };
        var vm = new InventoryItemViewModel(item);

        Assert.Same(item, vm.Model);
    }

    [Fact]
    public void InstanceId_ReturnsModelInstanceId()
    {
        var item = new InventoryItem();
        var vm = new InventoryItemViewModel(item);

        Assert.Equal(item.InstanceId, vm.InstanceId);
    }
}

public class InventoryCacheViewModelTests
{
    [Fact]
    public void Constructor_SetsLocationFromModel()
    {
        var cache = new InventoryCache { Location = "Mars Habitat" };

        var vm = new InventoryCacheViewModel(cache);

        Assert.Equal("Mars Habitat", vm.Location);
    }

    [Fact]
    public void Constructor_CreatesItemViewModels()
    {
        var cache = new InventoryCache
        {
            Location = "Test",
            Inventory = new List<InventoryItem>
            {
                new() { Name = "Item 1" },
                new() { Name = "Item 2" }
            }
        };

        var vm = new InventoryCacheViewModel(cache);

        Assert.Equal(2, vm.Items.Count);
        Assert.Equal("Item 1", vm.Items[0].Name);
        Assert.Equal("Item 2", vm.Items[1].Name);
    }

    [Fact]
    public void Location_UpdatesModel()
    {
        var cache = new InventoryCache { Location = "Original" };
        var vm = new InventoryCacheViewModel(cache);

        vm.Location = "Updated Location";

        Assert.Equal("Updated Location", cache.Location);
    }

    [Fact]
    public void AddItem_AddsToModelAndCollection()
    {
        var cache = new InventoryCache();
        var vm = new InventoryCacheViewModel(cache);
        var item = new InventoryItem { Name = "New Item" };

        vm.AddItem(item);

        Assert.Single(cache.Inventory);
        Assert.Single(vm.Items);
        Assert.Equal("New Item", vm.Items[0].Name);
    }

    [Fact]
    public void RemoveItem_RemovesFromModelAndCollection()
    {
        var cache = new InventoryCache
        {
            Inventory = new List<InventoryItem> { new() { Name = "Item" } }
        };
        var vm = new InventoryCacheViewModel(cache);
        var itemVm = vm.Items[0];

        vm.RemoveItem(itemVm);

        Assert.Empty(cache.Inventory);
        Assert.Empty(vm.Items);
    }

    [Fact]
    public void Model_ReturnsUnderlyingCache()
    {
        var cache = new InventoryCache { Location = "Test" };
        var vm = new InventoryCacheViewModel(cache);

        Assert.Same(cache, vm.Model);
    }
}

public class EgoManagerInventoryTests
{
    private readonly EgoManager _manager = new();

    [Fact]
    public void AddInventoryItem_AddsToEgo()
    {
        var ego = new Ego();

        var item = _manager.AddInventoryItem(ego, "Test Item", 3);

        Assert.Single(ego.Inventory);
        Assert.Equal("Test Item", item.Name);
        Assert.Equal(3, item.Quantity);
    }

    [Fact]
    public void RemoveInventoryItem_RemovesFromEgo()
    {
        var ego = new Ego();
        var item = _manager.AddInventoryItem(ego);

        var result = _manager.RemoveInventoryItem(ego, item);

        Assert.True(result);
        Assert.Empty(ego.Inventory);
    }

    [Fact]
    public void AddCache_AddsCacheToEgo()
    {
        var ego = new Ego();

        var cache = _manager.AddCache(ego, "Mars Safehouse");

        Assert.Single(ego.Caches);
        Assert.Equal("Mars Safehouse", cache.Location);
    }

    [Fact]
    public void RemoveCache_RemovesCacheFromEgo()
    {
        var ego = new Ego();
        var cache = _manager.AddCache(ego);

        var result = _manager.RemoveCache(ego, cache);

        Assert.True(result);
        Assert.Empty(ego.Caches);
    }

    [Fact]
    public void AddItemToCache_AddsItemToCache()
    {
        var cache = new InventoryCache();

        var item = _manager.AddItemToCache(cache, "Cached Item", 2);

        Assert.Single(cache.Inventory);
        Assert.Equal("Cached Item", item.Name);
        Assert.Equal(2, item.Quantity);
    }

    [Fact]
    public void RemoveItemFromCache_RemovesItemFromCache()
    {
        var cache = new InventoryCache();
        var item = _manager.AddItemToCache(cache);

        var result = _manager.RemoveItemFromCache(cache, item);

        Assert.True(result);
        Assert.Empty(cache.Inventory);
    }

    [Fact]
    public void MoveItemToCache_MovesFromEgoToCache()
    {
        var ego = new Ego();
        var item = _manager.AddInventoryItem(ego, "Moving Item");
        var cache = _manager.AddCache(ego);

        var result = _manager.MoveItemToCache(ego, item, cache);

        Assert.True(result);
        Assert.Empty(ego.Inventory);
        Assert.Single(cache.Inventory);
        Assert.Same(item, cache.Inventory[0]);
    }

    [Fact]
    public void MoveItemFromCache_MovesFromCacheToEgo()
    {
        var ego = new Ego();
        var cache = _manager.AddCache(ego);
        var item = _manager.AddItemToCache(cache, "Cached Item");

        var result = _manager.MoveItemFromCache(ego, item, cache);

        Assert.True(result);
        Assert.Single(ego.Inventory);
        Assert.Empty(cache.Inventory);
        Assert.Same(item, ego.Inventory[0]);
    }

    [Fact]
    public void MoveItemToCache_ReturnsFalseIfItemNotInEgo()
    {
        var ego = new Ego();
        var cache = _manager.AddCache(ego);
        var item = new InventoryItem { Name = "Not in ego" };

        var result = _manager.MoveItemToCache(ego, item, cache);

        Assert.False(result);
        Assert.Empty(cache.Inventory);
    }

    [Fact]
    public void MoveItemFromCache_ReturnsFalseIfItemNotInCache()
    {
        var ego = new Ego();
        var cache = _manager.AddCache(ego);
        var item = new InventoryItem { Name = "Not in cache" };

        var result = _manager.MoveItemFromCache(ego, item, cache);

        Assert.False(result);
        Assert.Empty(ego.Inventory);
    }
}
