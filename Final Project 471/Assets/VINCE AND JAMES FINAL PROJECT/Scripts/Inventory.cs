using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<ItemSO> items = new List<ItemSO>();

    // ✅ Add item to inventory
    public void AddItem(ItemSO item)
    {
        items.Add(item);
        Debug.Log(item.itemName + " added to inventory.");
    }

    // ✅ Remove all items (used after delivery maybe)
    public void Clear()
    {
        items.Clear();
    }

    // ✅ Check if an item exists in inventory (match by itemName)
    public bool HasItem(ItemSO item)
    {
        return items.Exists(i => i.itemName == item.itemName);
    }

    // ✅ Remove N copies of an item (match by itemName)
    public void RemoveItem(ItemSO item, int amount)
    {
        int removed = 0;
        for (int i = items.Count - 1; i >= 0 && removed < amount; i--)
        {
            if (items[i].itemName == item.itemName)
            {
                items.RemoveAt(i);
                removed++;
            }
        }

        if (removed == 0)
        {
            Debug.LogWarning("Tried to remove item that wasn't in inventory: " + item.itemName);
        }
    }

    // ✅ Startup debug
    void Start()
    {
        Debug.Log("🧺 Inventory Init: " + items.Count + " items.");
        foreach (var item in items)
        {
            Debug.Log(" - " + item.itemName);
        }
    }
}
