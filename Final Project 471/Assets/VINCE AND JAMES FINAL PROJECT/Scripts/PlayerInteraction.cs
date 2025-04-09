using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private KitchenStation nearbyStation;
    private Inventory inventory;

    void Start()
    {
        inventory = GetComponent<Inventory>();
    }

    void Update()
    {
        if (nearbyStation != null && !nearbyStation.IsBusy())
        {
            if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame) // 'A' on controller
            {
                nearbyStation.TryProcessItem(inventory);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out KitchenStation station))
        {
            nearbyStation = station;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out KitchenStation station) && station == nearbyStation)
        {
            nearbyStation = null;
        }
    }
}
