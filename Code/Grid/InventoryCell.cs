using Godot;
using GodotGridInventory.Code.Grid.Enums;
using GodotGridInventory.Code.UI;

namespace GodotGridInventory.Code.Grid;

#nullable enable
public class InventoryCell
{
    private readonly InventoryGrid _parentGrid;
    public Vector2 Position { get; set; }
    public EnumInventoryGridCellState State { get; set; }
    public Item? Item { get; set; } = null;
    public InventoryGridItem? ItemGraphic { get; set; } = null;
    
    
    public InventoryCell(Vector2 position, InventoryGrid parentGrid)
    {
        _parentGrid = parentGrid;
        State = EnumInventoryGridCellState.Available;
        Item = null;
        Position = position;
        GD.Print($"Inventory cell created at position {position.X}, {position.Y}");
    }
}