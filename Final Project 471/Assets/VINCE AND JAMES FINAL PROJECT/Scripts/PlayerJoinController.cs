using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class PlayerJoinController : MonoBehaviour
{
    public GameObject fighterPrefab; // Player 1 prefab (Fighter)
    public GameObject chefPrefab;    // Player 2 prefab (Chef)

    void Start()
    {
        // Unpair devices before reassigning
        InputUser.PerformPairingWithDevice(Keyboard.current, default);
        InputUser.PerformPairingWithDevice(Mouse.current, default);
        if (Gamepad.current != null)
        {
            InputUser.PerformPairingWithDevice(Gamepad.current, default);
        }

        // --- Player 1 (Fighter): Keyboard + Mouse ---
        var keyboardUser = InputUser.CreateUserWithoutPairedDevices();
        InputUser.PerformPairingWithDevice(Keyboard.current, keyboardUser);
        InputUser.PerformPairingWithDevice(Mouse.current, keyboardUser);

        PlayerInput.Instantiate(
            fighterPrefab,
            controlScheme: "Keyboard&Mouse",
            pairWithDevices: new InputDevice[] { Keyboard.current, Mouse.current },
            playerIndex: 0
        );

        // --- Player 2 (Chef): Gamepad ---
        if (Gamepad.current != null)
        {
            var gamepadUser = InputUser.CreateUserWithoutPairedDevices();
            InputUser.PerformPairingWithDevice(Gamepad.current, gamepadUser);

            PlayerInput.Instantiate(
                chefPrefab,
                controlScheme: "Gamepad",
                pairWithDevices: new InputDevice[] { Gamepad.current },
                playerIndex: 1
            );
        }
        else
        {
            Debug.LogWarning("⚠ No gamepad found for Player 2 (Chef)");
        }
    }
}
