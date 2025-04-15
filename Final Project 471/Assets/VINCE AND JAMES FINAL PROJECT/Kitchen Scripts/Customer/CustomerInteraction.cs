using UnityEngine;
using UnityEngine.InputSystem;

public class CustomerInteraction : MonoBehaviour
{
    private Customer nearbyCustomer;
    private Inventory inventory;

    void Start()
    {
        inventory = GetComponent<Inventory>();
    }

    void Update()
    {
        // Interact with A or E
        if (nearbyCustomer != null &&
            ((Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame) ||
             (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)))
        {
            if (inventory.items.Count > 0)
            {
                ItemSO itemToServe = inventory.items[0];
                nearbyCustomer.TryServe(itemToServe);
                inventory.RemoveItem(itemToServe, 1);
            }
            else
            {
                Debug.Log("🚫 You have nothing to serve.");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Customer customer = other.GetComponent<Customer>();
        if (customer != null)
        {
            nearbyCustomer = customer;
            Debug.Log("👋 Near customer who wants: " + customer.GetOrderName());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Customer>() == nearbyCustomer)
        {
            Debug.Log("👋 Left customer.");
            nearbyCustomer = null;
        }
    }
}
