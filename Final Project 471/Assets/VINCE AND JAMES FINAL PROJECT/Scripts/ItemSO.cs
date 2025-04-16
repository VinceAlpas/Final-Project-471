using UnityEngine;

public enum ItemType
{
    NONE,
    Bun,
    RawMeat,
    CookedMeat,
    Burger
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public ItemType type;
    public Sprite icon; // optional, for inventory UI
}
