    using Godot;
using GodotGridInventory.Code.Grid;

namespace GodotGridInventory.Code.UI;

public partial class InventoryGrid:Control
{

    private Grid.InventoryGrid _inventoryGrid;
    private InventoryController _inventoryController;


    [Export] public Control GridContainer { get; set; }
    [Export] public PackedScene GridSlotResource { get; set; }
    
    public override void _Ready()
    {
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
                var gridSlot = GridSlotResource.Instantiate() as Control;
                gridSlot.Position = new Vector2(x * _inventoryController.CellSize, y * _inventoryController.CellSize);
                gridSlot.Size = new Vector2(_inventoryController.CellSize-1, _inventoryController.CellSize-1);
                GridContainer.AddChild(gridSlot);
            }
        }
    }

    public Vector2 ScreenCoordsToInventoryGridCoords(Vector2 cursorPos)
    {
        var x = Mathf.FloorToInt(cursorPos.X / _inventoryController.CellSize);
        var y = Mathf.FloorToInt(cursorPos.Y / _inventoryController.CellSize);
        return new Vector2(x, y);
    }
    
    public void AddItem(InventoryGridItem inventoryItemDragged, bool useScreenCoords = true)
    {
        if (useScreenCoords == false)
        {
            var result = _inventoryGrid.AddItem(inventoryItemDragged.Item.Id, inventoryItemDragged);
            if(result!=null)
            {
                var parent = inventoryItemDragged.GetParent();
                if(parent!=null)
                    parent.RemoveChild(inventoryItemDragged);
                AddChild(inventoryItemDragged);
                inventoryItemDragged.Position = (Vector2)result * _inventoryController.CellSize;
            }
            return;
        }
        
        var cursorPos = GetGlobalMousePosition();
        var gridCoords = ScreenCoordsToInventoryGridCoords(cursorPos);
        if (_inventoryGrid.AddItem(inventoryItemDragged.Item.Id, inventoryItemDragged, gridCoords)!=null)
        {
            var parent = inventoryItemDragged.GetParent();
            if(parent!=null)
                parent.RemoveChild(inventoryItemDragged);
            AddChild(inventoryItemDragged);
            inventoryItemDragged.Position = gridCoords * _inventoryController.CellSize;
        }
    }
}