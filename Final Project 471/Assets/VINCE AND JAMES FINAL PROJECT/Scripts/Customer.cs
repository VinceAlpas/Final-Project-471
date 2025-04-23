using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System.Collections;

public class Customer : MonoBehaviour
{
    [Header("Order")]
    public CustomerOrderSO orderData;

    [Header("References")]
    public Transform targetTable;
    public Transform exitPoint;
    public GameObject exclamationIcon;
    public TextMeshProUGUI orderText;  // <-- Shows the text instead of bubble

    [Header("Timers")]
    public float waitTime = 60f;
    public float eatTime = 10f;

    private NavMeshAgent agent;
    private bool isOrderTaken = false;
    private bool isEating = false;
    private bool hasReachedTable = false;
    private float timer = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (targetTable != null)
        {
            agent.SetDestination(targetTable.position);
        }
        else
        {
            Debug.LogError("❌ Customer: No targetTable assigned.");
        }

        exclamationIcon?.SetActive(false);
        orderText?.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!hasReachedTable && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            hasReachedTable = true;
            agent.isStopped = true;
            exclamationIcon?.SetActive(true);
        }

        if (isOrderTaken && !isEating)
        {
            timer += Time.deltaTime;
            if (timer >= waitTime)
            {
                Debug.Log("⏰ Customer left (waited too long)");
                Leave();
            }
        }
    }

    public void Interact()
    {
        if (!isOrderTaken)
        {
            isOrderTaken = true;
            timer = 0f;

            exclamationIcon?.SetActive(false);
            ShowOrderText();

            Debug.Log("📝 Order Taken: " + orderData.orderName);
        }
    }

    public void Deliver(ItemSO item)
    {
        if (isOrderTaken && !isEating && item == orderData.requiredItem)
        {
            StartCoroutine(ReceiveFoodAndLeave());
        }
        else
        {
            Debug.Log("❌ Wrong item or too early.");
        }
    }

    void ShowOrderText()
    {
        if (orderText != null)
        {
            orderText.text = orderData.orderName;
            orderText.gameObject.SetActive(true);
        }
    }

    IEnumerator ReceiveFoodAndLeave()
    {
        isEating = true;
        yield return new WaitForSeconds(eatTime);

        Leave();
    }

    public void Leave()
    {
        if (orderText != null)
            orderText.gameObject.SetActive(false);

        exclamationIcon?.SetActive(false);

        if (exitPoint != null)
        {
            agent.isStopped = false;
            agent.SetDestination(exitPoint.position);
            StartCoroutine(DestroyAfterExit());
        }
        else
        {
            Debug.LogWarning("⚠️ No exit point set.");
            Destroy(gameObject);
        }
    }

    IEnumerator DestroyAfterExit()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    public void SetTargetTable(Transform table)
    {
        targetTable = table;

        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (table != null)
        {
            agent.SetDestination(table.position);
        }
        else
        {
            Debug.LogError("❌ SetTargetTable: table was null.");
        }
    }

    public void ExitFromArea(Vector3 exitPosition)
    {
        agent.isStopped = false;
        agent.SetDestination(exitPosition);
        StartCoroutine(DestroyAfterExit());
    }

    public void MoveNext(Vector3 newPosition)
    {
        agent.isStopped = false;
        agent.SetDestination(newPosition);
    }

    public string GetOrderName()
    {
        return orderData != null ? orderData.orderName : "No Order";
    }
}
