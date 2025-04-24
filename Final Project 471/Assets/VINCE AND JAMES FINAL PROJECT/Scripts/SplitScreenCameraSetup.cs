using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class SplitScreenCameraSetup : MonoBehaviour
{
    private void Start()
    {
        PlayerInput playerInput = GetComponent<PlayerInput>();
        Camera cam = GetComponentInChildren<Camera>();

        if (cam == null)
        {
            Debug.LogWarning("No camera found on player prefab.");
            return;
        }

        // Assign vertical split based on player index
        if (playerInput.playerIndex == 0)
        {
            // Player 1 → left half
            cam.rect = new Rect(0f, 0f, 0.5f, 1f);
        }
        else if (playerInput.playerIndex == 1)
        {
            // Player 2 → right half
            cam.rect = new Rect(0.5f, 0f, 0.5f, 1f);
        }
    }
}
