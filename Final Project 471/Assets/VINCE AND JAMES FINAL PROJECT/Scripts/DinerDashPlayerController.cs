using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class DinerDashPlayerController : MonoBehaviour
{
    private Vector2 moveInput;
    private CharacterController controller;

    public float moveSpeed = 3f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void Update()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        if (move != Vector3.zero)
        {
            // Optional: face direction you're moving in
            transform.forward = move;
        }

        controller.Move(move * moveSpeed * Time.deltaTime);
    }
}
