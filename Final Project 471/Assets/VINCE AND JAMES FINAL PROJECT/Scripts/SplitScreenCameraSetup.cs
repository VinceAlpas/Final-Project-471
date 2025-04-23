using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class SplitScreenCameraSetup : MonoBehaviour
{
    private void Start()
    {
        PlayerInput playerInput = GetComponent<PlayerInput>();
        Camera mainCam = Camera.main;

        if (mainCam == null)
        {
            Debug.LogWarning("No Main Camera found in scene.");
            return;
        }

        // Assign split-screen rect based on player index
        if (playerInput.playerIndex == 0)
        {
            // Player 1 → left half
            mainCam.rect = new Rect(0f, 0f, 0.5f, 1f);
        }
        else if (playerInput.playerIndex == 1)
        {
            // Player 2 → right half
            mainCam.rect = new Rect(0.5f, 0f, 0.5f, 1f);
        }
    }
}
