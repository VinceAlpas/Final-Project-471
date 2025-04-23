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
    [SerializeField] private List<Transform> tablePositions = new List<Transform>();

    private float currentTime = 0f;
    private float spawnCooldown = 0f;

    private HashSet<int> occupiedTables = new HashSet<int>();
    private Dictionary<Customer, int> customerTableMap = new Dictionary<Customer, int>();

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (customerPrefabs.Count == 0 || tablePositions.Count == 0)
            return;

        if (occupiedTables.Count >= tablePositions.Count)
            return; // all tables full

        spawnCooldown += Time.deltaTime * timerSpeed;

        if (spawnCooldown >= Random.Range(10f, 20f))
        {
            spawnCooldown = 0f;

            int tableIndex = GetAvailableTableIndex();
            if (tableIndex == -1)
            {
                Debug.Log("🚫 No available tables.");
                return;
            }

            Transform table = tablePositions[tableIndex];
            int randomIndex = Random.Range(0, customerPrefabs.Count);
            Vector3 spawnPos = spawnPoint.position;

            Customer newCustomer = Instantiate(customerPrefabs[randomIndex], spawnPos, Quaternion.identity);
            newCustomer.SetTargetTable(table); // You must define this in Customer.cs
            newCustomer.exitPoint = exitPoint; // Optional, if not already set via prefab

            customerList.Enqueue(newCustomer);
            occupiedTables.Add(tableIndex);
            customerTableMap[newCustomer] = tableIndex;
        }
    }

    private int GetAvailableTableIndex()
    {
        for (int i = 0; i < tablePositions.Count; i++)
        {
            if (!occupiedTables.Contains(i))
                return i;
        }
        return -1; // no tables available
    }

    public void CustomerFinished(Customer customer)
    {
        if (customerTableMap.TryGetValue(customer, out int tableIndex))
        {
            occupiedTables.Remove(tableIndex);
            customerTableMap.Remove(customer);
        }

        if (customerList.Contains(customer))
            customerList.Dequeue();
    }

    public void SellToCustomer()
    {
        if (customerList.Count == 0) return;

        Customer firstCustomer = customerList.Peek();
        firstCustomer.ExitFromArea(exitPoint.position);
        customerList.Dequeue();

        CustomerFinished(firstCustomer);
    }
}
