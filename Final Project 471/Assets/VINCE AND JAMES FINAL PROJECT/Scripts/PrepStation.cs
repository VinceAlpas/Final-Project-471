using UnityEngine;
using UnityEngine.InputSystem;

public class PrepStation : MonoBehaviour
{
    [Header("Prep Settings")]
    public ItemSO rawIngredient;
    public ItemSO preppedIngredient;
    public int pressesRequired = 10;
    public float interactionRange = 2f;

    private Inventory currentInventory;
    private Transform currentPlayer;
    private int currentPresses = 0;
    private bool isPrepping = false;
    private bool playerWasInZone = false;

    void Update()
    {
        if (currentPlayer != null && currentInventory != null)
        {
            float distance = Vector3.Distance(transform.position, currentPlayer.position);

            if (distance <= interactionRange)
            {
                // ✅ Player is in zone
                if (!playerWasInZone)
                {
                    // ✨ Re-enter detected
                    Debug.Log("📦 Entered Prep Station: " + currentPlayer.name);

                    playerWasInZone = true;

                    if (currentInventory.HasItem(rawIngredient))
                    {
                        StartPrepping();
                    }
                    else
                    {
                        Debug.Log("⚠️ No raw ingredients in inventory.");
                    }
                }

                HandleMashInput();
            }
            else
            {
                // ❌ Player left the zone
                if (playerWasInZone)
                {
                    Debug.Log("⬅️ Player walked away from prep station — resetting.");
                    ResetState();
                }
            }
        }
    }

    void HandleMashInput()
    {
        bool mashPressed =
            (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame) ||
            (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame);

        if (isPrepping && mashPressed)
        {
            currentPresses++;
            Debug.Log("🔨 Mash Count: " + currentPresses + " / " + pressesRequired);

            if (currentPresses >= pressesRequired)
            {
                currentInventory.RemoveItem(rawIngredient, 1);
                currentInventory.AddItem(preppedIngredient);

                Debug.Log("✅ Finished prepping " + rawIngredient.name + " → added " + preppedIngredient.name);

                currentPresses = 0;
                isPrepping = false;

                if (currentInventory.HasItem(rawIngredient))
                {
                    StartPrepping();
                }
                else
                {
                    Debug.Log("🍽️ No more ingredients.");
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (currentInventory == null && other.CompareTag("Player"))
        {
            currentInventory = other.GetComponent<Inventory>();
            currentPlayer = other.transform;
            playerWasInZone = false; // force re-entry detection

            // Will log in Update when first distance check runs
        }
    }

    void ResetState()
    {
        currentPresses = 0;
        isPrepping = false;
        playerWasInZone = false;
    }

    void StartPrepping()
    {
        currentPresses = 0;
        isPrepping = true;
        Debug.Log("🍄 Started prepping " + rawIngredient.name);
    }
}
