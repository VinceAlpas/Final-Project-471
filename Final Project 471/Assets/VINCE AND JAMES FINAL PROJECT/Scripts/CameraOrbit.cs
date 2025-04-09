using UnityEngine;
using UnityEngine.InputSystem;

public class CameraOrbit : MonoBehaviour
{
    public Transform target; // Assign the player (Chef)
    public float rotationSpeed = 100f;

    private Vector2 lookInput;

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Always follow the target's position
        transform.position = target.position;

        // Rotate around the Y-axis using right stick
        if (lookInput.sqrMagnitude > 0.1f)
        {
            float horizontal = lookInput.x * rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up, horizontal);
        }
    }
}
