using UnityEngine;
using UnityEngine.UI;

public class CookingTable : MonoBehaviour
{
    public float cookTime = 10f;
    private float holdTimer = 0f;
    public Inventory inventory; // Reference to player's inventory
    public ItemSO rawMeat; // Reference to raw meat item
    public ItemSO cookedMeat; // Reference to cooked meat item
    public GameObject uiProgressBar;
    public Image progressFill;

    private bool playerInZone = false;

    private void Update()
    {
        if (playerInZone && Input.GetKey(KeyCode.JoystickButton0)) // A button
        {
            if (inventory.HasItem(rawMeat)) // Ensure the player has raw meat
            {
                holdTimer += Time.deltaTime;
                progressFill.fillAmount = holdTimer / cookTime;

                if (holdTimer >= cookTime)
                {
                    inventory.RemoveItem(rawMeat, 1); // Remove raw meat from inventory
                    inventory.AddItem(cookedMeat); // Add cooked meat to inventory
                    Debug.Log("✅ Meat cooked!");
                    holdTimer = 0;
                    progressFill.fillAmount = 0;
                    uiProgressBar.SetActive(false);
                }
            }
            else
            {
                Debug.Log("❌ You need raw meat to cook.");
                holdTimer = 0;
                progressFill.fillAmount = 0;
                uiProgressBar.SetActive(false);
            }
        }
        else if (!Input.GetKey(KeyCode.JoystickButton0))
        {
            // Reset if player lets go
            holdTimer = 0;
            progressFill.fillAmount = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inventory = other.GetComponent<Inventory>();
            playerInZone = true;
            uiProgressBar.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            uiProgressBar.SetActive(false);
            holdTimer = 0;
            progressFill.fillAmount = 0;
        }
    }
}
