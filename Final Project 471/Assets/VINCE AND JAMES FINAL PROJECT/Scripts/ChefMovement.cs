using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class ChefMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public Transform cameraTransform;
    private Vector2 lookInput;
    private Vector2 moveInput;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null)
        {
            Camera cam = GetComponentInChildren<Camera>();
            if (cam != null)
            {
                cameraTransform = cam.transform;
            }
            else
            {
                Debug.LogError("ChefMovement: cameraTransform not assigned and no child camera found.");
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
   {
       moveInput = context.ReadValue<Vector2>();
     //   Debug.Log("ChefMovement: move input = " + moveInput);
  }
public void OnLook(InputAction.CallbackContext context)
{
    lookInput = context.ReadValue<Vector2>();
}
   void Update()
{
    if (cameraTransform == null) return;

    // Calculate movement direction relative to camera
    Vector3 camForward = cameraTransform.forward;
    Vector3 camRight = cameraTransform.right;
    camForward.y = 0f;
    camRight.y = 0f;
    camForward.Normalize();
    camRight.Normalize();

    Vector3 moveDir = camForward * moveInput.y + camRight * (moveInput.x * 0.3f);
    moveDir.y = 0f;

    // Movement
    if (moveDir.sqrMagnitude > 0.01f)
    {
        Vector3 moveVector = moveDir.normalized * moveSpeed * Time.deltaTime;
        controller.Move(moveVector);

        // Smoothly rotate player to face movement direction
        Quaternion targetRotation = Quaternion.LookRotation(moveDir.normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}





}
