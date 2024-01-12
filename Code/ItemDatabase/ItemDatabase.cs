using System.Linq;
using Godot;
using Godot.Collections;

namespace GodotGridInventory.Code;

public class ItemDatabase
{
    public static ItemDatabase Instance { get; } = new();
    private readonly Dictionary<string, Item> _itemDatabase = new();
    
    public ItemDatabase()
    {
        InitializeItemDatabase();
    }   
    
    public Item GetItemConfiguration(string id)
    {
        return _itemDatabase[id];
    }
    
    private void InitializeItemDatabase()
    {
        GD.Print("Initializing the item database.");
        var items = DirAccess.GetFilesAt(ItemDatabaseConstants.ITEMS_PATH).Where(filePath => filePath.EndsWith(ItemDatabaseConstants.ITEM_EXTENSION));
        foreach (var item in items)
        {
            var itemResource = GD.Load<Item>(item);
            if (itemResource != null)
            {
                _itemDatabase.Add(itemResource.Id, itemResource);
                GD.Print($"Loaded item {itemResource.Name} with id {itemResource.Id} into the item database.");
            }
        }
    }
}