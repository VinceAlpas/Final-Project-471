using UnityEngine;

public class DungeonDeliveryZone : MonoBehaviour
{
    public KitchenItemBuffer kitchenBuffer;

    private void OnTriggerEnter(Collider other)
    {
        Inventory fighterInventory = other.GetComponent<Inventory>();

        if (fighterInventory != null && fighterInventory.items.Count > 0)
        {
            foreach (var item in fighterInventory.items)
            {
                kitchenBuffer.AddItem(item);
            }

            fighterInventory.Clear();
            Debug.Log("Items teleported to kitchen buffer.");
        }
    }
}
