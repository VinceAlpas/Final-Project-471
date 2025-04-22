using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public GameObject bloodEffectPrefab;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy Hit!");

            if (bloodEffectPrefab != null)
            {
                Instantiate(bloodEffectPrefab, other.transform.position, Quaternion.identity);
            }

            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage(10); // Or however much you want to deal
            }

            // GameManager call should now happen from within EnemyAI.Die()
            // So no need to call it here directly
        }
    }
}
