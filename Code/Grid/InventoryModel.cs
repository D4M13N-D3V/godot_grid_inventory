using System.Collections.Generic;
using System.Linq;
using Godot;
using GodotGridInventory.Code.UI;

namespace GodotGridInventory.Code.Grid;

#nullable enable
public class InventoryModel
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int GridWidth { get; set; }
    public int GridHeight { get; set; }
    public List<Item> Items { get; set; } = new List<Item>();
    public List<InventoryCell> Cells { get; set; } = new List<InventoryCell>();
    public InventoryGrid? Grid { get; set; } = null;
    public UI.InventoryGrid? GridInterface { get; set; } = null;
    public bool Open { get; set; } = false;
}