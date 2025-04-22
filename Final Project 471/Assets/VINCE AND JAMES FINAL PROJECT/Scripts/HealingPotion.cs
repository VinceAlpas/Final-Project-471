using UnityEngine;

public class HealingPotion : MonoBehaviour
{
    public int healAmount = 25;

    private void OnTriggerEnter(Collider other)
    {
        fpshealth playerHealth = other.GetComponent<fpshealth>();
        if (playerHealth != null)
        {
            playerHealth.Heal(healAmount);
            Destroy(gameObject); // Remove potion after use
        }
    }
}
