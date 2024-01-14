using System.Collections.Generic;
using System.Linq;
using Godot;
using GodotGridInventory.Code.Grid.Enums;
using GodotGridInventory.Code.UI;

namespace GodotGridInventory.Code.Grid;

#nullable enable
public class InventoryGrid
{
    private readonly InventoryController _inventoryController;
    public readonly Vector2 GridSize;
    
    public List<InventoryCell> GridCells = new();
    
    public InventoryGrid(InventoryController inventoryController, Vector2 size)
    {
        _inventoryController = inventoryController;
        GridSize = size;
        InitializeGrid();
    }
    
    public InventoryGrid(InventoryController inventoryController, List<InventoryCell> gridCells)
    {
        _inventoryController = inventoryController;
        GridCells = gridCells;
    }

    private void InitializeGrid()
    {
        for(var x = 0; x < GridSize.X; x++)
        {
            for(var y = 0; y < GridSize.Y; y++)
            {
                var cellPosition = new Vector2(x, y);
                var cell = new InventoryCell(cellPosition, this);
                GridCells.Add(cell);
            }
        }
    }
    
    #region Private Methods
    /// <summary>
    /// Sets the item of a cell at a given position.
    /// </summary>
    /// <param name="position">The position of the cell to update.</param>
    /// <param name="item">The item to set the cell to.</param>
    private void SetCellItem(Vector2 position, Item item)
    {
        var cell = GridCells.FirstOrDefault(cell => cell.Position == position);
        if(cell != null)
        {
            cell.Item = item;
        }
    }
    
    /// <summary>
    /// Clears the item of a cell at a given position.
    /// </summary>
    /// <param name="position">The position of the cell to update.</param>
    private void ClearCellItem(Vector2 position)
    {
        var cell = GridCells.FirstOrDefault(cell => cell.Position == position);
        if(cell != null)
        {
            cell.Item = null;
        }
    }
    
    /// <summary>
    /// Sets the state of a cell at a given position in inventory grid space.
    /// </summary>
    /// <param name="position">The position of the cell to set.</param>
    /// <param name="state">The state to set the cell</param>
    private void SetCellState(Vector2 position, EnumInventoryGridCellState state)
    {
        var cell = GridCells.FirstOrDefault(cell => cell.Position == position);
        if(cell != null)
        {
            cell.State = state;
        }
    }
    
    /// <summary>
    /// Sets the state of a selection of cells at a given position and size in inventory grid space.
    /// </summary>
    /// <param name="position">The position of the cells to set from the top left..</param>
    /// <param name="size">The size of the area to set starting at the top left.</param>
    /// <param name="state">The state to set the cells</param>
    private void SetCellAreaState(Vector2 position, Vector2 size, EnumInventoryGridCellState state)
    {
        var cells = GridCells.Where(cell =>
            cell.Position.X >= position.X && cell.Position.X < position.X + size.X && cell.Position.Y >= position.Y &&
            cell.Position.Y < position.Y + size.Y).ToList();
        foreach (var cell in cells)
        {
            cell.State = state;
        }
    }
    
    /// <summary>
    /// Sets the item of a selection of cells at a given position and size in inventory grid space.
    /// </summary>
    /// <param name="position">The position of the cells to set from the top left..</param>
    /// <param name="size">The size of the area to set starting at the top left.</param>
    /// <param name="item">The item to set the cells</param>
    private void SetCellAreaItem(Vector2 position, Vector2 size, Item item, InventoryGridItem itemGraphic)
    {
        var cells = GridCells.Where(cell =>
            cell.Position.X >= position.X && cell.Position.X < position.X + size.X && cell.Position.Y >= position.Y &&
            cell.Position.Y < position.Y + size.Y).ToList();
        foreach (var cell in cells)
        {
            cell.Item = item;
            cell.ItemGraphic = itemGraphic;
        }
    }
    
    /// <summary>
    /// Gets the first available cell position that can fit an item of a given size.
    /// </summary>
    /// <param name="size">The size to check for.</param>
    /// <returns>The first coordinates available for something of the given size, or null if no space available.</returns>
    private Vector2? GetAvailableCellPosition(Vector2 size)
    {
        for (int x = 0; x <= GridSize.X - size.X; x++)
        {
            for (int y = 0; y <= GridSize.Y - size.Y; y++)
            {
                // Check if the area is available
                if (IsSpaceAvailable(new Vector2(x, y), size))
                {
                    return new Vector2(x,y);
                }
            }
        }

        return null;
    }
    
    /// <summary>
    /// This checks all of the cells in a space to see if all of them are available.
    /// </summary>
    /// <param name="position">The top left position of the item in the inventories grid space.</param>
    /// <param name="size">The size of the item in inventory grid space.</param>
    /// <returns>A List of cells that are available for the item to be placed in.</returns>
    private bool IsSpaceAvailable(Vector2 position, Vector2 size)
    {
        var cells = GridCells.Where( cell => cell.Position.X >= position.X && cell.Position.X < position.X + size.X && cell.Position.Y >= position.Y && cell.Position.Y < position.Y + size.Y);
        return cells.All(cell => cell.State == EnumInventoryGridCellState.Available);
    }
    #endregion
    
    #region Public Methods
    
    /// <summary>
    /// Adds an item to the inventory grid.
    /// </summary>
    /// <param name="itemId">The ID of the item to add.</param>
    /// <param name="position">The position to add the item at in inventory grid space. If null then will insert at first available place.</param>
    /// <returns></returns>
    public Vector2? AddItem(string itemId, InventoryGridItem itemGraphic, Vector2? position = null)
    {
        var item = ItemDatabase.Instance.GetItemConfiguration(itemId);
        if(item == null)
        {
            GD.PrintErr("Attempted to add item with id " + itemId + ", which does not exist.");
            return null;
        }
        
        if(position.HasValue==false)
        {
            position = GetAvailableCellPosition(item.Size);
        }

        if (position.HasValue==false)
        {
            GD.PrintErr("Attempted to add item with id " + itemId + ", but there is no space available.");
            return null;
        }
        
        SetCellAreaState((Vector2)position, item.Size, EnumInventoryGridCellState.Unavailable);
        SetCellAreaItem((Vector2)position, item.Size, item, itemGraphic);
        return position;
    }
    
    /// <summary>
    /// Gets the cell at the given location in inventory grid space.
    /// </summary>
    /// <param name="position">The position you want to get the cell of.</param>
    /// <returns>The inventory cell at the given location in inventory grid space.</returns>
    public InventoryCell? GetCell(Vector2 position)
    {
        return GridCells.FirstOrDefault(cell => cell.Position == position);
    }
    #endregion
    
}