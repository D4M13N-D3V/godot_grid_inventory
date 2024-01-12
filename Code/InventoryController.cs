using System.Collections.Generic;
using System.Linq;
using Godot;
using GodotGridInventory.Code.Grid;

namespace GodotGridInventory.Code;

public partial class InventoryController : Node
{
    private int _inventoryIdCounter = 0;
    private readonly ItemDatabase _itemDatabase = ItemDatabase.Instance;
    [Export] public float CellSize { get; set; } = 32;
    private Dictionary<int,InventoryModel> _inventories { get; set; } = new Dictionary<int,InventoryModel>();
    
    public override void _Ready()
    {
        base._Ready();
    }

    public bool UpdateInventory(int id, InventoryModel inventoryModel)
    {
        var inventory = _inventories[id];
        if(inventory == null)
        {
            GD.PrintErr($"Inventory not found with id {id}.");
            return false;
        }

        _inventories[id] = inventoryModel;
        return true;
    }

    public InventoryModel GetInventory(int id)
    {
        var inventory = _inventories[id];
        if(inventory == null)
        {
            GD.PrintErr($"Inventory not found with id {id}.");
            return null;
        }

        var result = _inventories[id];
        return result;
    }
    
    public void AddInventory(string name, Vector2 size, string[] items)
    {
        var inventoryGrid = new InventoryGrid(this, size);
        
        foreach (var item in items)
        {
            inventoryGrid.AddItem(item);
        }
        
        _inventories.Add(_inventoryIdCounter, new InventoryModel()
        {
            Id = _inventoryIdCounter,
            Name = name,
            Items = items.Select( item=> _itemDatabase.GetItemConfiguration(item)).ToList(),
            Cells = inventoryGrid.GridCells,
            Open = false
        });
        
        _inventoryIdCounter++;
    }

    public bool RemoveInventory(int Id)
    {
        var inventory = _inventories[Id];
        if(inventory == null)
        {
            GD.PrintErr($"Inventory not found with id {Id}.");
            return false;
        }

        _inventories.Remove(Id);
        GD.Print($"Inventory {inventory.Name} with id {inventory.Id} removed.");
        return true;
    }
    
    public bool OpenInventory(int Id)
    {
        var inventory = _inventories[Id];
        if(inventory == null)
        {
            GD.PrintErr($"Inventory not found with id {Id}.");
            return false;
        }

        if (inventory.Open == true)
        {
            GD.PrintErr($"Inventory with {Id} is already open.");
            return false;
        }
        _inventories[Id].Open = true;
        GD.Print($"Opened inventory {inventory.Name} with id {inventory.Id}.");
        return true;
    }
    
    public bool CloseInventory(int Id)
    {
        if(!_inventories.ContainsKey(Id))
        {
            GD.PrintErr("Inventory not found with id " + Id + ".");
            return false;
        }

        var inventory = _inventories[Id];
        if (inventory.Open == false)
        {
            GD.PrintErr($"Inventory with {Id} is already closed.");
            return false;
        }
        
        _inventories[Id].Open = false;
        _inventories.Remove(Id);
        GD.Print("Closed inventory " + inventory.Name + " with id " + inventory.Id + ".");
        return true;
    }
    
    public List<InventoryModel> GetInventoriesLoaded()
    {
        var result = _inventories.Values.ToList();
        GD.Print($"Getting list of all _inventoriesLoaded loaded into memory ({result.Count}).");
        return result;
    }
}