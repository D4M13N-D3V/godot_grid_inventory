    using Godot;
using GodotGridInventory.Code.Grid;

namespace GodotGridInventory.Code.UI;

public partial class InventoryGrid:Control
{

    private PackedScene _gridSlotResource;
    private Grid.InventoryGrid _inventoryGrid;
    private InventoryController _inventoryController;


    [Export] public Control GridContainer;
    [Export] public string GridSlotPath;
    
    public override void _Ready()
    {
        _gridSlotResource = GD.Load<PackedScene>(GridSlotPath);
        base._Ready();
    }

    public void InitializeInventoryGrid(Grid.InventoryGrid inventoryGrid, InventoryController inventoryController)
    {
        _inventoryGrid = inventoryGrid;
        _inventoryController = inventoryController;

        for (var x = 0; x < _inventoryGrid.GridSize.X; x++)
        {
            for (var y = 0; y < _inventoryGrid.GridSize.Y; y++)
            {
                var cellPosition = new Vector2(x, y);
                var cell = _inventoryGrid.GetCell(cellPosition);
                var gridSlot = _gridSlotResource.Instantiate() as Control;
                gridSlot.Position = new Vector2(x * _inventoryController.CellSize, y * _inventoryController.CellSize);
                GridContainer.AddChild(gridSlot);
            }
        }
    }

    private Vector2 ScreenCoordsToInventoryGridCoords(Vector2 cursorPos)
    {
        var x = Mathf.FloorToInt(cursorPos.X / _inventoryController.CellSize);
        var y = Mathf.FloorToInt(cursorPos.Y / _inventoryController.CellSize);
        return new Vector2(x, y);
    }
    
    public void AddItem(InventoryGridItem inventoryItemDragged)
    {
        var cursorPos = GetGlobalMousePosition();
        var gridCoords = ScreenCoordsToInventoryGridCoords(cursorPos);
        if (_inventoryGrid.AddItem(inventoryItemDragged.Item.Id, gridCoords))
        {
            inventoryItemDragged.Position = gridCoords * _inventoryController.CellSize;
            AddChild(inventoryItemDragged);
        }
    }
}