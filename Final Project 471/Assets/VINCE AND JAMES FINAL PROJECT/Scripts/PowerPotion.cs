using UnityEngine;

public class PowerPotion : MonoBehaviour
{
    public int bonusDamage = 20;
    public float duration = 80f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SwordAttack sword = other.GetComponentInChildren<SwordAttack>(); // Adjust as needed
            if (sword != null)
            {
                sword.ActivatePowerBoost(bonusDamage, duration);
            }

            Destroy(gameObject); // Remove the potion after pickup
        }
    }
}
