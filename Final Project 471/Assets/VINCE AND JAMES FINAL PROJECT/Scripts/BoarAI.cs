using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BoarAI : MonoBehaviour
{
    [Header("Boar Settings")]
    public int health = 100;
    public int damage = 10;
    public float detectionRange = 10f;
    public float chargeSpeed = 10f;
    public float idleSpeed = 2f;
    public float chargeCooldown = 3f;

    [Header("Drop Settings")]
    public ItemSO dropItem;                     // 🧺 Item to drop
    public GameObject itemDropPrefab;           // 📦 Prefab with ItemPickup

    [Header("References")]
    public Transform player;                    // 🔎 Assigned automatically at runtime

    private Rigidbody rb;
    private bool isCharging = false;
    private float lastChargeTime = -999f;
    private Vector3 roamTarget;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (player == null)
        {
            GameObject found = GameObject.FindGameObjectWithTag("Player");
            if (found != null) player = found.transform;
        }

        roamTarget = GetNewRoamPoint();
    }

    void Update()
    {
        if (health <= 0 || player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (!isCharging && distanceToPlayer <= detectionRange && Time.time > lastChargeTime + chargeCooldown)
        {
            StartCoroutine(ChargeAtPlayer());
        }
        else if (!isCharging)
        {
            Roam();
        }
    }

    void Roam()
    {
        Vector3 direction = (roamTarget - transform.position).normalized;
        rb.MovePosition(transform.position + direction * idleSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, roamTarget) < 1f)
        {
            roamTarget = GetNewRoamPoint();
        }
    }

    Vector3 GetNewRoamPoint()
    {
        Vector2 rand = Random.insideUnitCircle * 5f;
        return new Vector3(transform.position.x + rand.x, transform.position.y, transform.position.z + rand.y);
    }

    System.Collections.IEnumerator ChargeAtPlayer()
    {
        isCharging = true;
        lastChargeTime = Time.time;

        Vector3 direction = (player.position - transform.position).normalized;

        float chargeTime = 1.5f;
        float elapsed = 0f;

        while (elapsed < chargeTime)
        {
            rb.MovePosition(transform.position + direction * chargeSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        isCharging = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            fpshealth playerHealth = collision.gameObject.GetComponent<fpshealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("💥 Grovel Boar slammed into player for " + damage + " damage.");
            }
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("💀 Grovel Boar defeated!");

        if (dropItem != null && itemDropPrefab != null)
        {
            GameObject drop = Instantiate(itemDropPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            ItemPickup pickup = drop.GetComponent<ItemPickup>();
            if (pickup != null)
            {
                pickup.itemData = dropItem; 
            }
        }

        Destroy(gameObject);
    }
}
