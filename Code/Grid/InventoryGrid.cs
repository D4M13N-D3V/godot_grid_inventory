using System.Collections.Generic;
using System.Linq;
using Godot;
using GodotGridInventory.Code.Grid.Enums;

namespace GodotGridInventory.Code.Grid;

public class InventoryGrid
{
    private readonly int _gridWidth;
    private readonly int _gridHeight;
    
    private readonly List<InventoryCell> _gridCells = new();
    
    public InventoryGrid(int width, int height)
    {
        _gridWidth = width;
        _gridHeight = height;
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        for(var x = 0; x < _gridWidth; x++)
        {
            for(var y = 0; y < _gridHeight; y++)
            {
                var cellPosition = new Vector2(x, y);
                var cell = new InventoryCell(cellPosition, this);
                _gridCells.Add(cell);
            }
        }
    }
    
    #region Private Methods
    /// <summary>
    /// Sets the item of a cell at a given position.
    /// </summary>
    /// <param name="position">The position of the cell to update.</param>
    /// <param name="item">The item to set the cell to.</param>
    private void SetCellItem(Vector2 position, ItemConfiguration item)
    {
        var cell = _gridCells.FirstOrDefault(cell => cell.Position == position);
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
        var cell = _gridCells.FirstOrDefault(cell => cell.Position == position);
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
        var cell = _gridCells.FirstOrDefault(cell => cell.Position == position);
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
        foreach (var cell in _gridCells.Where(cell => cell.Position.X >= position.X && cell.Position.X < position.X + size.X && cell.Position.Y >= position.Y && cell.Position.Y < position.Y + size.Y))
        {
            cell.State = state;
        }
    }
    
    /// <summary>
    /// Gets the first available cell position that can fit an item of a given size.
    /// </summary>
    /// <param name="size">The size to check for.</param>
    /// <returns>The first coordinates available for something of the given size, or null if no space available.</returns>
    private Vector2? GetAvailableCellPosition(Vector2 size)
    {
        var availableCells = _gridCells.Where(cell => cell.State == EnumInventoryGridCellState.Available);
        var cell = availableCells.FirstOrDefault();
        if(cell != null)
        {
            return cell.Position;
        }
        
        for (int x = 0; x <= _gridWidth - size.X; x++)
        {
            for (int y = 0; y <= _gridHeight - size.Y; y++)
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
        var cells = _gridCells.Where( cell => cell.Position.X >= position.X && cell.Position.X < position.X + size.X && cell.Position.Y >= position.Y && cell.Position.Y < position.Y + size.Y);
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
    public bool AddItem(string itemId, Vector2? position)
    {
        var item = ItemDatabase.Instance.GetItemConfiguration(itemId);
        if(item == null)
        {
            GD.PrintErr("Attempted to add item with id " + itemId + ", which does not exist.");
            return false;
        }
        
        if(position.HasValue==false)
        {
            position = GetAvailableCellPosition(item.Size);
        }

        if (position.HasValue==false)
        {
            GD.PrintErr("Attempted to add item with id " + itemId + ", but there is no space available.");
            return false;
        }
        
        SetCellAreaState((Vector2)position, item.Size, EnumInventoryGridCellState.Unavailable);
        return true;
    }
    
    /// <summary>
    /// Gets the cell at the given location in inventory grid space.
    /// </summary>
    /// <param name="position">The position you want to get the cell of.</param>
    /// <returns>The inventory cell.</returns>
    public InventoryCell? GetCell(Vector2 position)
    {
        return _gridCells.FirstOrDefault(cell => cell.Position == position);
    }
    #endregion
    
}