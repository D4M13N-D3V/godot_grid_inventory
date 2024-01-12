using System;

namespace GodotGridInventory.Code.Items.Enums;

[Flags]
public enum EnumItemActions
{
    None = 0,
    Examine = 1 << 1,
    Equip = 1 << 2,
    Use = 1 << 3,
    Open = 1 << 4,
    Drop = 1 << 5,
    Destroy = 1 << 6
}