using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Transform target; // Assigned via SetTarget()
    public Vector3 offset = new Vector3(0, 1.0f, 0);
    public Image fillImage;

    private Camera mainCam;

    void Start()
{
    float closestDistance = Mathf.Infinity;
    foreach (Camera cam in Camera.allCameras)
    {
        if (!cam.enabled) continue;

        float dist = Vector3.Distance(transform.position, cam.transform.position);
        if (dist < closestDistance)
        {
            mainCam = cam;
            closestDistance = dist;
        }
    }
}


    void Update()
    {
        if (target == null || mainCam == null) return;

        // Position above enemy
        transform.position = target.position + offset;

        // Face the camera
        transform.LookAt(mainCam.transform);
    }

    public void UpdateHealthBar(float current, float max)
    {
        if (fillImage == null || max <= 0) return;

        fillImage.fillAmount = Mathf.Clamp01(current / max);
    }

    // ✅ Call this after spawning the health bar to assign target
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
