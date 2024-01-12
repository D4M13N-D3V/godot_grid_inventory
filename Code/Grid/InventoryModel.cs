using System.Collections.Generic;
using System.Linq;
using Godot;

namespace GodotGridInventory.Code.Grid;
public class InventoryModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int GridWidth { get; set; }
    public int GridHeight { get; set; }
    
    public List<Item> Items { get; set; }

    public List<InventoryCell> Cells { get; set; } = new List<InventoryCell>();
    public InventoryGrid? Grid { get; set; }
    public bool Open { get; set; } = false;
}