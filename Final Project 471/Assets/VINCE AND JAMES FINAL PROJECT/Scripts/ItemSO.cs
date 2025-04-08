using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public Sprite icon; // optional, for inventory UI
}
