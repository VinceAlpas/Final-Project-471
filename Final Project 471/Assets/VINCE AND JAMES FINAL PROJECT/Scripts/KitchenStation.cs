using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class KitchenStation : MonoBehaviour
{
    [Header("Cooking Settings")]
    public ItemSO acceptedInput;
    public ItemSO resultItem;
    public float processingTime = 5f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip dingSound;

    [Header("UI")]
    public Image circularTimer;
    public TextMeshProUGUI boilPromptText;
    public TextMeshProUGUI collectPromptText;

    private float timer;
    private bool isProcessing = false;
    private bool isCooked = false;

    private bool playerNearby = false;
    private Inventory currentInventory;
    private GameObject currentPlayer;

    void Start()
    {
        if (circularTimer != null)
            circularTimer.gameObject.SetActive(false);

        HideAllPrompts();
    }

    void Update()
    {
        // Handle cooking progress
        if (isProcessing)
        {
            timer += Time.deltaTime;

            if (circularTimer != null)
            {
                float progress = Mathf.Clamp01(1f - (timer / processingTime));
                circularTimer.fillAmount = progress;
            }

            if (timer >= processingTime)
            {
                FinishCooking();
            }
        }

        // Handle input
        if (playerNearby && Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            if (isCooked)
            {
                GiveCookedItem();
            }
            else if (!isProcessing && currentInventory != null && currentInventory.HasItem(acceptedInput))
            {
                StartCooking();
                HideAllPrompts();
            }
        }
    }

    private void StartCooking()
    {
        currentInventory.RemoveItem(acceptedInput, 1);
        timer = 0f;
        isProcessing = true;
        isCooked = false;

        if (circularTimer != null)
        {
            circularTimer.fillAmount = 1;
            circularTimer.gameObject.SetActive(true);
        }

        Debug.Log("✅ Cooking started.");
    }

    private void FinishCooking()
    {
        isProcessing = false;
        isCooked = true;

        if (circularTimer != null)
            circularTimer.fillAmount = 0;

        if (audioSource != null && dingSound != null)
            audioSource.PlayOneShot(dingSound);

        Debug.Log("✅ Cooking complete. Awaiting collection.");
        ShowCollectPrompt();
    }

    private void GiveCookedItem()
    {
        if (currentInventory != null && resultItem != null)
        {
            currentInventory.AddItem(resultItem);
            Debug.Log("🧺 Collected: " + resultItem.itemName);
        }

        isCooked = false;

        if (circularTimer != null)
            circularTimer.gameObject.SetActive(false);

        // ✅ Check first, then clear reference
        bool hasMoreIngredients = currentInventory != null && currentInventory.HasItem(acceptedInput);
        currentInventory = null;

        if (hasMoreIngredients)
            ShowBoilPrompt();
        else
            HideAllPrompts();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currentInventory = other.GetComponent<Inventory>();
            currentPlayer = other.gameObject;
            playerNearby = true;

            if (isCooked)
            {
                ShowCollectPrompt();
            }
            else if (!isProcessing && currentInventory.HasItem(acceptedInput))
            {
                ShowBoilPrompt();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentPlayer)
        {
            playerNearby = false;
            currentPlayer = null;
            currentInventory = null;

            HideAllPrompts();
        }
    }

    private void ShowBoilPrompt()
    {
        if (boilPromptText != null) boilPromptText.gameObject.SetActive(true);
        if (collectPromptText != null) collectPromptText.gameObject.SetActive(false);
    }

    private void ShowCollectPrompt()
    {
        if (collectPromptText != null) collectPromptText.gameObject.SetActive(true);
        if (boilPromptText != null) boilPromptText.gameObject.SetActive(false);
    }

    private void HideAllPrompts()
    {
        if (boilPromptText != null) boilPromptText.gameObject.SetActive(false);
        if (collectPromptText != null) collectPromptText.gameObject.SetActive(false);
    }

    public bool IsBusy() => isProcessing || isCooked;

    public bool TryProcessItem(Inventory playerInventory)
    {
        if (!isProcessing && !isCooked && playerInventory.HasItem(acceptedInput))
        {
            currentInventory = playerInventory;
            StartCooking();
            HideAllPrompts();
            return true;
        }

        return false;
    }
}
