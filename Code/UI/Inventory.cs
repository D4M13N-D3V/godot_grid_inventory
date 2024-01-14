using Godot;
using GodotGridInventory.Code.Grid;

namespace GodotGridInventory.Code.UI;

public class Inventory: Control
{
    private InventoryGridItem _inventoryItemDragged = null;
    private Vector2 _inventoryItemCursorOffset = Vector2.Zero;
    private InventoryGrid _inventoryItemDraggedLastContainer = null;
    private Vector2 _inventoryItemDraggedLastPos = Vector2.Zero;
    private InventoryController _inventoryController;
    
    [Export] public string InventoryDragActionName = "inventory_drag";
    [Export] public string InventoryDropActionName = "inventory_release";
    [Export] public string InventoryRotateActionName = "inventory_rotate";
    
    
    #region Private Methods

    private InventoryModel GetInventoryUnderCursor(Vector2 cursor_pos)
    {
        GD.Print("Checking for inventory under cursor.");
        foreach (var inventory in _inventoryController.GetInventoriesLoaded())
        {
            if (inventory.GridInterface.GetGlobalRect().HasPoint(cursor_pos))
            {
                //todo expected behaviour is if they are overlapping the one with the lower ID will take priority this may need to be reversed.
                GD.Print($"Inventory found under cursor with ID {inventory.Id}.");
                return inventory; 
            }
        }
        GD.Print("No inventory found under cursor.");
        return null;
    }

    private void Grab(Vector2 cursorPos)
    {
        var inventory = GetInventoryUnderCursor(cursorPos);
        if (inventory != null)
        {
            var inventoryItem = inventory.Grid?.GetCell(cursorPos).ItemGraphic;
            if (inventoryItem != null)
            {
                _inventoryItemDragged = inventoryItem;
                _inventoryItemDraggedLastContainer = inventory.GridInterface;
                _inventoryItemDraggedLastPos = _inventoryItemDragged.GlobalPosition;
                _inventoryItemCursorOffset = _inventoryItemDragged.GlobalPosition - cursorPos;
            }
        }
    }
    
    private void Release(Vector2 cursorPos)
    {
        if (_inventoryItemDragged == null)
            return;
        
        var inventory = GetInventoryUnderCursor(cursorPos);
        if (inventory == null)
        {
            ReturnItem();
            return;
        }
    }

    private void ReturnItem()
    {
        _inventoryItemDragged.GlobalPosition = _inventoryItemDraggedLastPos;
        _inventoryItemDraggedLastContainer.AddItem(_inventoryItemDragged);
        _inventoryItemDragged = null;
    }
    #endregion

    #region Public Methods
    public void InitializeInventory(InventoryController inventoryController)
    {
        _inventoryController = inventoryController;
    }
    #endregion
    
    #region Overrides

    public override void _Ready()
    {
        base._Ready();
    }

    public override void _Process(double delta)
    {
        if(Input.IsActionJustPressed(InventoryRotateActionName) && _inventoryItemDragged != null)
            _inventoryItemDragged.RotationDegrees+= 90.0f;
        
        if(_inventoryItemDragged != null && _inventoryItemDragged.RotationDegrees==360.0f)
            _inventoryItemDragged.RotationDegrees=0.0f;

        var cursor_pos = GetGlobalMousePosition();
	
        if (Input.IsActionJustPressed(InventoryDragActionName))
        {
            Grab(cursor_pos);
        }
			
        if (Input.IsActionJustReleased(InventoryDragActionName))
        {
            Release(cursor_pos);
        }
        base._Process(delta);
    }
    #endregion
}