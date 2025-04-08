using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemSO itemData; // Assign in Inspector

    private void OnTriggerEnter(Collider other)
    {
        Inventory inventory = other.GetComponent<Inventory>();
        if (inventory != null)
        {
            inventory.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}
