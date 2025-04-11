using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectnType
{
    public GameObject item;
    public ItemType type;
}

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<ObjectnType> itemsToHold = new List<ObjectnType>(); 
    private ItemType currentType;
    public ItemType CurrentType { get { return currentType; } }
    
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
        currentType = ItemType.NONE;

        Debug.Log("🧺 Inventory Init: " + items.Count + " items.");
        foreach (var item in items)
        {
            Debug.Log(" - " + item.itemName);
        }
    }

     public void TakeItem(ItemType type)
    {
        if (currentType != ItemType.NONE) return;
        currentType = type;
        foreach(ObjectnType itemHold in itemsToHold)
        {
            if (itemHold.type != type)
            {
                itemHold.item.SetActive(false);
            }
            else
            {
                itemHold.item.SetActive(true);
            }
        }
    }

    public ItemType PutItem()
    {
        if (currentType == ItemType.NONE) return ItemType.NONE; 
        return currentType;

    }
    public void ClearHand()
    {
        currentType = ItemType.NONE; 
        itemsToHold.ForEach(obj => obj.item.SetActive(false));
    }

    public ItemType GetItem()
    {
        return currentType;
    }
}
