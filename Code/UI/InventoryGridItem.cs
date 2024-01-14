using Godot;

namespace GodotGridInventory.Code.UI;

public partial class InventoryGridItem: Control
{
    public InventoryGridItem(Item item)
    {
        Item = item;
    }
    public Item Item { get; } = null;
}