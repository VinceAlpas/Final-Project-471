using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public GameObject bloodEffectPrefab;
    private GameManager gameManager;

    public int baseDamage = 15;
    private int currentDamage;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        currentDamage = baseDamage; // Start with base damage
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
                enemy.TakeDamage(currentDamage); // Use current damage
            }
        }
    }

    public void ActivatePowerBoost(int bonusDamage, float duration)
    {
        StopAllCoroutines(); // Stop any previous boost timers
        StartCoroutine(PowerBoostCoroutine(bonusDamage, duration));
    }

    private System.Collections.IEnumerator PowerBoostCoroutine(int bonusDamage, float duration)
    {
        currentDamage = baseDamage + bonusDamage;
        Debug.Log("Power boost active! Damage: " + currentDamage);
        yield return new WaitForSeconds(duration);
        currentDamage = baseDamage;
        Debug.Log("Power boost ended. Damage reverted to: " + currentDamage);
    }
}
