using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Kitchen/Item Buffer")]
public class KitchenItemBuffer : ScriptableObject
{
    public List<ItemSO> bufferedItems = new List<ItemSO>();

    public void AddItem(ItemSO item)
    {
        bufferedItems.Add(item);
    }

    public void Clear()
    {
        bufferedItems.Clear();
    }

    public bool HasItems()
    {
        return bufferedItems.Count > 0;
    }
}
