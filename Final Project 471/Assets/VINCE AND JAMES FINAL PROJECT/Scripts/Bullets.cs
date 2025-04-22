using UnityEngine;

public class Bullets : MonoBehaviour
{
    public float speed = 20f;  // Bullet speed
    private float shootingDuration = 5f;  // Time bullets can be shot (in seconds)
    private float cooldownDuration = 30f;  // Time to wait before shooting again (in seconds)
    private float lastShotTime;  // Time when shooting started
    private bool isShooting = false;  // Track whether shooting is active
    private bool canShoot = true;  // Whether the player can shoot or not

    public GameObject bulletPrefab;  // Reference to the Bullet prefab (to instantiate bullets)
    public Transform shootingPoint;  // The point from which bullets will be fired

    void Update()
    {
        // If the player holds the right-click button and can shoot
        if (Input.GetMouseButton(1) && canShoot && !isShooting)  
        {
            StartShooting();
        }

        // If the cooldown period has passed, allow shooting again
        if (!canShoot && Time.time - lastShotTime >= cooldownDuration)
        {
            canShoot = true;  // Allow shooting after cooldown
        }
    }

    void StartShooting()
    {
        // Start shooting and record the time when shooting begins
        isShooting = true;
        lastShotTime = Time.time;  
        Invoke("StopShooting", shootingDuration);  // Stop shooting after 5 seconds
        ShootBullet();  // Spawn a bullet
    }

    void StopShooting()
    {
        // Stop shooting after 5 seconds and begin cooldown
        isShooting = false;
        canShoot = false;  // Start cooldown
        Debug.Log("Shooting stopped. Cooling down...");
    }

    void ShootBullet()
    {
        // Only spawn a bullet if the shooting is active (within the 5-second window)
        if (bulletPrefab != null && shootingPoint != null)
        {
            Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);  // Spawn bullet at the shooting point
        }
    }

    void Start()
    {
        lastShotTime = Time.time;  // Initialize last shot time
    }
}
