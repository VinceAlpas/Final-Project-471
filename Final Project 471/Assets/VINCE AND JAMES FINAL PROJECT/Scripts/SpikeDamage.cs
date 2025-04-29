using System.Collections;
using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    public int damageAmount = 10;
    public float damageCooldown = 1.0f; // Time in seconds between damage ticks
    private float lastDamageTime;

    private GameObject player;
    private fpshealth playerHealth;
    private Renderer playerRenderer;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<fpshealth>();
            playerRenderer = player.GetComponent<Renderer>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageAmount);
                    StartCoroutine(BlinkRed());
                    lastDamageTime = Time.time;
                }
            }
        }
    }

    private IEnumerator BlinkRed()
    {
        if (playerRenderer != null)
        {
            Color originalColor = playerRenderer.material.color;
            playerRenderer.material.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            playerRenderer.material.color = originalColor;
        }
    }


    void ResetColor(GameObject player)
    {
        Renderer[] renderers = player.GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in renderers)
        {
            // Assumes player is originally white; adjust if needed
            rend.material.color = Color.white;
        }
    }
}

