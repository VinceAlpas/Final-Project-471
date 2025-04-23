using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Transform target; // Enemy
    public Vector3 offset = new Vector3(0, 1.0f, 0); // Above the enemy
    public Image fillImage;

    void Update()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
            transform.LookAt(Camera.main.transform);
        }
    }

    public void UpdateHealthBar(float current, float max)
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = current / max;
        }
    }
}

