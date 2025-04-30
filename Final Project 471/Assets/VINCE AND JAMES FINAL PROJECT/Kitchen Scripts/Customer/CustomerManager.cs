using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager Instance;

    [Header("Customer Setup")]
    [SerializeField] private Queue<Customer> customerList = new Queue<Customer>();
    [SerializeField] private List<Customer> customerPrefabs = new List<Customer>();
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform exitPoint;
    [SerializeField] private List<Transform> tablePositions = new List<Transform>();

    [Header("Spawn Timing")]
    [SerializeField] private float minSpawnDelay = 3f;
    [SerializeField] private float maxSpawnDelay = 8f;

    private HashSet<int> occupiedTables = new HashSet<int>();
    private Dictionary<Customer, int> customerTableMap = new Dictionary<Customer, int>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // ✅ Spawn customers to fill tables at game start
        for (int i = 0; i < tablePositions.Count; i++)
        {
            StartCoroutine(SpawnCustomerWithDelay(Random.Range(minSpawnDelay, maxSpawnDelay)));
        }
    }

    private int GetAvailableTableIndex()
    {
        for (int i = 0; i < tablePositions.Count; i++)
        {
            if (!occupiedTables.Contains(i))
                return i;
        }
        return -1;
    }

    private void TrySpawnCustomer()
    {
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
        newCustomer.SetTargetTable(table);
        newCustomer.exitPoint = exitPoint;

        customerList.Enqueue(newCustomer);
        occupiedTables.Add(tableIndex);
        customerTableMap[newCustomer] = tableIndex;

        Debug.Log("👤 Customer spawned at table " + tableIndex);
    }

    private IEnumerator SpawnCustomerWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        TrySpawnCustomer();
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

        // ✅ Wait random delay before trying to spawn next
        StartCoroutine(SpawnCustomerWithDelay(Random.Range(minSpawnDelay, maxSpawnDelay)));
    }

    public void SellToCustomer()
    {
        if (customerList.Count == 0) return;

        Customer firstCustomer = customerList.Peek();
        firstCustomer.ExitFromArea(exitPoint.position);
        customerList.Dequeue();

        CustomerFinished(firstCustomer);

        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.CustomerServed();
        }
        else
        {
            Debug.LogWarning("GameManager not found when trying to increment chef's served count.");
        }
    }
}
