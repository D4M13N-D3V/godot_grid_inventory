using Godot;

namespace GodotGridInventory.Code.UI;

public partial class InventoryGridItem: Control
{
    private InventoryController _inventoryController;
    [Export] private TextureRect Texture { get; set; } = null;
    public Item Item { get; set; } = null;

    public void  InitializeItem(Item item, InventoryController inventoryController)
    {
        _inventoryController = inventoryController;;
        Item = item;
        Texture.Texture = item.Texture;
        Texture.Size = new Vector2(Item.Size.X*_inventoryController.CellSize, Item.Size.Y*_inventoryController.CellSize);
    }
}