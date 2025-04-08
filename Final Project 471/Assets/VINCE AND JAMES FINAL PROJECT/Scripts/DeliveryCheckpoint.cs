using UnityEngine;

public class DeliveryCheckpoint : MonoBehaviour
{
    public Transform kitchenDropSpawn; // Where items appear
    public GameObject pickupPrefabTemplate; // Generic pickup prefab with ItemPickup script

    private void OnTriggerEnter(Collider other)
    {
        Inventory fighterInventory = other.GetComponent<Inventory>();

        if (fighterInventory != null && fighterInventory.items.Count > 0)
        {
            foreach (var item in fighterInventory.items)
            {
                if (pickupPrefabTemplate != null && kitchenDropSpawn != null)
                {
                    GameObject pickup = Instantiate(pickupPrefabTemplate, kitchenDropSpawn.position, Quaternion.identity);

                    // Set the item data on the new pickup
                    ItemPickup pickupScript = pickup.GetComponent<ItemPickup>();
                    if (pickupScript != null)
                    {
                        pickupScript.itemData = item;
                    }
                }
            }

            fighterInventory.Clear();
            Debug.Log("Delivered items have spawned in the kitchen.");
        }
    }
}
