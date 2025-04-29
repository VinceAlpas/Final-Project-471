using UnityEngine;

public class FallDoorControl : MonoBehaviour
{
    private Vector3 initialPosition;
    private bool isFalling = false;
    private Rigidbody rb;  // To reference the Rigidbody

    void Start()
    {
        // Store the initial position of the falldoor
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        rb.isKinematic = true; // Initially set the Rigidbody to be kinematic so it won't fall until triggered
    }

    void Update()
    {
        // Check if the player presses "O" and is in contact with the falldoor
        if (Input.GetKeyDown(KeyCode.O) && isFalling == false)
        {
            Collider playerCollider = GetComponent<Collider>();
            if (playerCollider != null && playerCollider.bounds.Intersects(GetComponent<Collider>().bounds))
            {
                FallDown();
            }
        }
    }

    void FallDown()
    {
        // Set Rigidbody to not be kinematic, enabling gravity and physics interactions
        rb.isKinematic = false;
        rb.useGravity = true; // Allow gravity to affect the falldoor
        isFalling = true;

        // Optionally, apply additional force or effect (e.g., for an extra push)
        // rb.AddForce(Vector3.down * 10f, ForceMode.Impulse);

        Invoke("Respawn", 5f);  // Wait 5 seconds before respawning
    }

    void Respawn()
    {
        // Respawn the falldoor to its original position
        transform.position = initialPosition;
        rb.isKinematic = true; // Make it kinematic again so it doesn't fall until triggered
        rb.useGravity = false; // Disable gravity until the next fall
        isFalling = false;
    }
}
