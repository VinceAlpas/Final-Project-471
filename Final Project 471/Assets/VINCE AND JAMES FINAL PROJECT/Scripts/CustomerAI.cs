using UnityEngine;
using TMPro;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections;

public class CustomerAI : MonoBehaviour
{
    public CustomerOrderSO orderData;

    [Header("Visuals")]
    public GameObject exclamationUI;
    public GameObject thoughtBubbleUI;
    public Image thoughtBubbleImage;
    public Sprite happyFace;
    public TextMeshProUGUI orderText;

    [Header("Timing")]
    public float waitTime = 60f;
    public float eatDuration = 10f;

    [Header("Movement")]
    public Transform targetTable;
    private NavMeshAgent agent;
    private bool orderTaken = false;
    private bool isEating = false;
    private float orderTimer = 0f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(targetTable.position);

        exclamationUI.SetActive(false);
        thoughtBubbleUI.SetActive(false);
    }

    private void Update()
    {
        if (orderTaken && !isEating)
        {
            orderTimer += Time.deltaTime;
            if (orderTimer >= waitTime)
            {
                Debug.Log("⏱️ Customer got tired of waiting and left.");
                Leave();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Table"))
        {
            agent.isStopped = true;
            exclamationUI.SetActive(true); // Show ! when ready to interact
        }
    }

    public void Interact()
    {
        if (!orderTaken)
        {
            orderTaken = true;
            exclamationUI.SetActive(false);
            ShowThoughtBubble();
            Debug.Log("📝 Order Taken: " + orderData.orderName);
        }
    }

    public void DeliverFood(ItemSO deliveredItem)
    {
        if (orderTaken && !isEating && deliveredItem == orderData.requiredItem)
        {
            Debug.Log("✅ Food matched order! Customer is happy.");
            StartCoroutine(EatAndLeave());
        }
    }

    void ShowThoughtBubble()
    {
        if (thoughtBubbleUI != null)
        {
            thoughtBubbleUI.SetActive(true);
            thoughtBubbleImage.sprite = orderData.orderIcon;
            orderText.text = orderData.orderName;
        }
    }

    IEnumerator EatAndLeave()
    {
        isEating = true;
        orderTimer = 0f;

        thoughtBubbleImage.sprite = happyFace;
        orderText.text = "Yum!";
        yield return new WaitForSeconds(eatDuration);

        Leave();
    }

    void Leave()
    {
        Debug.Log("🚶 Customer is leaving.");
        Destroy(gameObject);
    }
}
