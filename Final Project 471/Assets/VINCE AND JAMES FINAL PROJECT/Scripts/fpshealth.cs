using UnityEngine;
using UnityEngine.UI;  // Import UI for health bar interaction
using UnityEngine.SceneManagement;

public class fpshealth : MonoBehaviour
{
    public int health = 100;
    private CameraShake cameraShake;

    // Health bar UI components
    public Image healthBarFill;  // Reference to the health bar's fill image

    void Start()
    {
        cameraShake = Camera.main.GetComponent<CameraShake>();
        UpdateHealthBar(); // Ensure the health bar starts at full
    }

    // Method to update health bar UI based on the current health
    void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = health / 100f;  // Set the fill amount based on health
        }
    }

    public void Heal(int amount)
{
    health += amount;
    if (health > 100) health = 100; // Clamp to max health
    UpdateHealthBar();
    Debug.Log("Player healed! Health: " + health);
}

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Player took damage! Health: " + health);

        // Shake camera when taking damage
        if (cameraShake != null)
        {
            cameraShake.Shake(0.1f, 1.0f); 
        }

        // Update the health bar
        UpdateHealthBar();

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Died!");
        // Reload the scene when player dies
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
