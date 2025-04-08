using UnityEngine;

public class KitchenReceiveZone : MonoBehaviour
{
    public KitchenItemBuffer kitchenBuffer;

    private void OnTriggerEnter(Collider other)
    {
        Inventory chefInventory = other.GetComponent<Inventory>();

        if (chefInventory != null && kitchenBuffer.HasItems())
        {
            foreach (var item in kitchenBuffer.bufferedItems)
            {
                chefInventory.AddItem(item);
            }

            kitchenBuffer.Clear();
            Debug.Log("Chef received all transferred items.");
        }
    }
}
