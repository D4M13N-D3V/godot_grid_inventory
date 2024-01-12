using Godot;
using GodotGridInventory.Code.Grid;

namespace GodotGridInventory.Code.UI;

public partial class InventoryGridInterface:Control
{
    [Export] public Control GridContainer { get; set; }
    [Export] public string GridSlotPath { get; set; }

    private PackedScene _gridSlotResource;
    private InventoryGrid _inventoryGrid;
    private InventoryController _inventoryController;
    
    public override void _Ready()
    {
        _gridSlotResource = GD.Load<PackedScene>(GridSlotPath);
        base._Ready();
    }

    public void InitializeInventoryGrid(InventoryGrid inventoryGrid, InventoryController inventoryController)
    {
        _inventoryGrid = inventoryGrid;
        _inventoryController = inventoryController;

        for (var x = 0; x < _inventoryGrid.GridSize.X; x++)
        {
            for (var y = 0; y < _inventoryGrid.GridSize.Y; y++)
            {
                var cellPosition = new Vector2(x, y);
                var cell = _inventoryGrid.GetCell(cellPosition);
                var gridSlot = _gridSlotResource.Instantiate() as InventoryGridSlot;
                gridSlot.Position = new Vector2(x * _inventoryController.CellSize, y * _inventoryController.CellSize);
                GridContainer.AddChild(gridSlot);
            }
        }
    }
}