using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Customer : MonoBehaviour
{
    [SerializeField] private GameObject burger;
    public ItemSO desiredItem; // Assign MushroomStew.asset in Inspector
    private bool isServed = false;

    public void TryServe(ItemSO deliveredItem)
    {
        if (isServed)
        {
            Debug.Log("🙅 Customer already served.");
            return;
        }

        if (deliveredItem != null && deliveredItem.itemName == desiredItem.itemName)
        {
            Debug.Log("✅ Customer happily received: " + deliveredItem.itemName);
            isServed = true;

            // TODO: Add score, animation, despawn etc.
        }
        else
        {
            Debug.Log("❌ Wrong item. Customer still wants: " + desiredItem.itemName);
        }
    }

    public string GetOrderName()
    {
        return desiredItem != null ? desiredItem.itemName : "(none)";
    }

    public void ExitFromArea(Vector3 pos)
    {
        burger.SetActive(true);
        transform.DOMove(pos, 3).OnComplete(()=> { Destroy(this.gameObject);});
    }
    public void MoveNext(Vector3 pos)
    {
        transform.DOMove(pos, 2);
    }
}
