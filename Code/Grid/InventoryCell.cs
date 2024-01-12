using Godot;
using GodotGridInventory.Code.Grid.Enums;

namespace GodotGridInventory.Code.Grid;

public class InventoryCell
{
    private readonly InventoryGrid _parentGrid;
    public Vector2 Position { get; set; }
    public EnumInventoryGridCellState State { get; set; }
    public ItemConfiguration? Item { get; set; } = null;
    
    
    public InventoryCell(Vector2 position, InventoryGrid parentGrid)
    {
        _parentGrid = parentGrid;
        State = EnumInventoryGridCellState.Available;
        Item = null;
        Position = position;
    }
}