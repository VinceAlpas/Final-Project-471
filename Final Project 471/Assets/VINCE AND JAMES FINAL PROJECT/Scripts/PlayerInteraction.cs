using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private KitchenStation nearbyStation;
    private Customer nearbyCustomer;
    private Inventory inventory;

    void Start()
    {
        inventory = GetComponent<Inventory>();
    }

    void Update()
    {
        // Press A or fallback key
        bool interactPressed = Gamepad.current != null 
            ? Gamepad.current.buttonSouth.wasPressedThisFrame 
            : Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame;

        if (interactPressed)
        {
            // 🧂 Handle kitchen interaction
            if (nearbyStation != null)
            {
                nearbyStation.TryProcessItem(inventory);
            }

            // 🧍‍♂️ Handle customer interaction
            if (nearbyCustomer != null)
            {
                nearbyCustomer.Interact();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out KitchenStation station))
        {
            nearbyStation = station;
        }

        if (other.TryGetComponent(out Customer customer)) // ✅ FIXED
        {
            nearbyCustomer = customer;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out KitchenStation station) && station == nearbyStation)
        {
            nearbyStation = null;
        }

        if (other.TryGetComponent(out Customer customer) && customer == nearbyCustomer) // ✅ FIXED
        {
            nearbyCustomer = null;
        }
    }
}
