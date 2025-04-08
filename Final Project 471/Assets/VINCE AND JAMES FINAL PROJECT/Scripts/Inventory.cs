using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<ItemSO> items = new List<ItemSO>();

    // Add item to inventory
    public void AddItem(ItemSO item)
    {
        items.Add(item);
        Debug.Log(item.itemName + " added to inventory.");
    }

    // Clear all items from inventory (used after delivery)
    public void Clear()
    {
        items.Clear();
    }

    // Check if an item is in inventory (optional for cooking logic)
    public bool HasItem(string itemName)
    {
        return items.Exists(i => i.itemName == itemName);
    }
}
