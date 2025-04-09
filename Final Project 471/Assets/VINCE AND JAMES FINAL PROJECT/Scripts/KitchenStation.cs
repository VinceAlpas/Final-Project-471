using UnityEngine;

public class KitchenStation : MonoBehaviour
{
    public ItemSO acceptedInput;    // e.g., CutMushroom
    public ItemSO resultItem;       // e.g., MushroomStew
    public float processingTime = 5f;

    private float timer;
    private bool isProcessing = false;
    private Inventory currentInventory;

    void Update()
    {
        if (isProcessing)
        {
            timer += Time.deltaTime;

            if (timer >= processingTime)
            {
                CompleteProcessing();
            }
        }
    }

    public bool TryProcessItem(Inventory playerInventory)
    {
        if (isProcessing || !playerInventory.HasItem(acceptedInput))
        {
            Debug.Log("❌ Station busy or wrong item.");
            return false;
        }

        playerInventory.RemoveItem(acceptedInput, 1);
        currentInventory = playerInventory;
        timer = 0f;
        isProcessing = true;

        Debug.Log("✅ Started cooking " + acceptedInput.itemName);
        return true;
    }

    void CompleteProcessing()
    {
        isProcessing = false;

        if (currentInventory != null && resultItem != null)
        {
            currentInventory.AddItem(resultItem);
            Debug.Log("✅ Finished: " + resultItem.itemName);
        }

        currentInventory = null;
    }

    public bool IsBusy() => isProcessing;
}
