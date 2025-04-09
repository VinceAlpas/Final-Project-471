using UnityEngine;

public class Customer : MonoBehaviour
{
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
}
