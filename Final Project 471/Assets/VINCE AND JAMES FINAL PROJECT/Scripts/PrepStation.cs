using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PrepStation : MonoBehaviour
{
    [Header("Prep Settings")]
    public ItemSO rawIngredient;
    public ItemSO preppedIngredient;
    public TextMeshProUGUI mashCounterText;

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
                if (!playerWasInZone)
                {
                    Debug.Log("📦 Entered Prep Station: " + currentPlayer.name);
                    playerWasInZone = true;

                    if (currentInventory.HasItem(rawIngredient))
                    {
                        StartPrepping();
                    }
                    else
                    {
                        Debug.Log("⚠️ No raw ingredients in inventory.");
                        UpdateMashText(""); // Hide text if no ingredients
                    }
                }

                HandleMashInput();
            }
            else
            {
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
            UpdateMashText($"Mash Count: {currentPresses} / {pressesRequired}");

            if (currentPresses >= pressesRequired)
            {
                currentInventory.RemoveItem(rawIngredient, 1);
                currentInventory.AddItem(preppedIngredient);

                Debug.Log($"✅ Finished prepping {rawIngredient.name} → added {preppedIngredient.name}");

                currentPresses = 0;
                isPrepping = false;

                if (currentInventory.HasItem(rawIngredient))
                {
                    StartPrepping(); // next item
                }
                else
                {
                    Debug.Log("🍽️ No more ingredients.");
                    UpdateMashText(""); // clear UI
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
            playerWasInZone = false; // reset zone flag
        }
    }

    void ResetState()
    {
        currentPresses = 0;
        isPrepping = false;
        playerWasInZone = false;
        UpdateMashText(""); // hide UI
    }

    void StartPrepping()
    {
        currentPresses = 0;
        isPrepping = true;
        Debug.Log("🍄 Started prepping " + rawIngredient.name);
        UpdateMashText($"Mash Count: {currentPresses} / {pressesRequired}");
    }

    void UpdateMashText(string text)
    {
        if (mashCounterText != null)
        {
            mashCounterText.text = text;
        }
    }
}
