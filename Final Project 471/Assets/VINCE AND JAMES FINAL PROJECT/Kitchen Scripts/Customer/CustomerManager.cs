using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager Instance;

    [Header("Customer Timing")]
    [SerializeField] private float timerSpeed = 1f;

    [Header("Customer Setup")]
    [SerializeField] private Queue<Customer> customerList = new Queue<Customer>();
    [SerializeField] private List<Customer> customerPrefabs = new List<Customer>();
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform exitPoint;

    private float currentTime = 0f;
    private float lrRandom = 0.75f;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (customerPrefabs.Count == 0)
        {
            Debug.LogWarning("⚠️ CustomerManager: No customer prefabs assigned!");
            return;
        }

        if (currentTime <= Random.Range(50, 80)) // spawnRate is random between 50–80 seconds
        {
            currentTime += Time.deltaTime * timerSpeed;
        }
        else
        {
            currentTime = 0;

            Vector3 spawnPos = spawnPoint.position
                + (spawnPoint.forward * -1 * customerList.Count)
                + (spawnPoint.right * Random.Range(-lrRandom, lrRandom));

            int randomIndex = Random.Range(0, customerPrefabs.Count);
            Customer temp = Instantiate(customerPrefabs[randomIndex], spawnPos, spawnPoint.rotation);
            customerList.Enqueue(temp);
        }
    }

    public void SellToCustomer()
    {
        if (customerList.Count == 0) return;

        // Send first customer away
        Customer firstCustomer = customerList.Peek();
        firstCustomer.ExitFromArea(exitPoint.position);
        customerList.Dequeue();

        // Move remaining customers up in line
        Customer[] customers = customerList.ToArray();
        for (int i = 0; i < customers.Length; i++)
        {
            Vector3 nextPos = spawnPoint.position
                + (spawnPoint.forward * -1 * i)
                + (spawnPoint.right * Random.Range(-lrRandom, lrRandom));

            customers[i].MoveNext(nextPos);
        }
    }
}
