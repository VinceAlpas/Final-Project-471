using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerStateManager : MonoBehaviour
{
    [HideInInspector] public PlayerBaseState currentState;
    [HideInInspector] public PlayerIdleState idleState = new PlayerIdleState();
    [HideInInspector] public PlayerWalkState walkState = new PlayerWalkState();
    [HideInInspector] public PlayerSneakState sneakState = new PlayerSneakState();
    [HideInInspector] public PlayerSprintState sprintState = new PlayerSprintState();
    [HideInInspector] public PlayerJumpState jumpState = new PlayerJumpState();
    [HideInInspector] public PlayerAttackState attackState = new PlayerAttackState();
    [HideInInspector] public PlayerDoubleJumpState doubleJumpState = new PlayerDoubleJumpState();

    [HideInInspector] public Vector2 movement;
    public Vector2 lookInput;

    public Transform playerCamera;
    public float mouseSensitivity = 100f;
    public float xRotation = 0f;

    public float default_speed = 2f;
    public float sprint_speed = 3f;
    public float sneak_speed = 0.5f;
    public float jumpForce = 0.3f;
    public bool isSneaking = false;
    public bool isSprinting = false;
    public bool IsJumping = false;
    public bool isAttacking = false;
    public CharacterController controller;
    public Vector3 velocity;
    public float gravity = -20f;
    public float verticalVelocity;
    public bool isGrounded;

    private bool hasDoubleJumped = false;
    private float lastJumpTime = -100f;
    public float doubleJumpWindow = 0.3f;

    public GameObject sword;
    public Animator animator;

    public Transform bulletSpawner;
    public GameObject bulletPrefab;
    private bool isFiring = false;
    private float fireRate = 0.1f;
    private float lastFireTime = 0f;

    public float fallThreshold = -10f;
    public int playerHealth = 100;

    private PlayerBaseState previousState;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        animator = GetComponent<Animator>();
        SwitchState(idleState);
    }

    void Update()
    {

        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        currentState.UpdateState(this);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        HandleMouseLook();
        HandleShooting();

        if (transform.position.y < fallThreshold)
            Die();
    }

    void OnMove(InputValue moveVal)
    {
        movement = moveVal.Get<Vector2>();
        Debug.Log("🎮 MOVE input: " + movement);

        if (isAttacking) return;

        if (movement.magnitude > 0.1f)
        {
            if (isSprinting)
                SwitchState(sprintState);
            else if (isSneaking)
                SwitchState(sneakState);
            else
                SwitchState(walkState);
        }
        else
        {
            SwitchState(idleState);
        }
    }

    void OnLook(InputValue lookVal)
    {
        lookInput = lookVal.Get<Vector2>();
    }

    void HandleMouseLook()
    {
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        if (playerCamera != null)
            playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    void OnSneak(InputValue sneakVal)
    {
        isSneaking = sneakVal.isPressed;

        if (isSneaking && !isSprinting)
            SwitchState(sneakState);
        else if (!isSneaking && !isSprinting)
            SwitchState(walkState);
    }

    void OnSprint(InputValue sprintVal)
    {
        isSprinting = sprintVal.isPressed;

        if (isSprinting && !isSneaking)
            SwitchState(sprintState);
        else if (!isSprinting && !isSneaking)
            SwitchState(walkState);
    }

    void OnJump()
    {
        if (isGrounded)
        {
            JumpPlayer();
            lastJumpTime = Time.time;
            hasDoubleJumped = false;
        }
        else if (Time.time - lastJumpTime <= doubleJumpWindow && !hasDoubleJumped)
        {
            SwitchState(doubleJumpState);
            doubleJumpState.RecordJumpTime();
        }
    }

    void OnAttack()
    {
        if (!isAttacking)
        {
            Debug.Log("Sword Attack!");
            isAttacking = true;

            animator.SetTrigger("Attack");
            SwitchState(attackState);

            Invoke("EndAttack", 0.5f);
        }
    }

    void EndAttack()
    {
        isAttacking = false;
        if (movement.magnitude > 0.1f)
            SwitchState(walkState);
        else
            SwitchState(idleState);
    }

    public void MovePlayer(float speed)
    {
        Vector3 forward = playerCamera.forward;
        Vector3 right = playerCamera.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 move = forward * movement.y + right * movement.x;

        if (controller != null)
            controller.Move(move * speed * Time.deltaTime);
    }

    public void SwitchState(PlayerBaseState newState)
    {
        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    void HandleShooting()
    {
        if (Mouse.current.rightButton.isPressed && Time.time > lastFireTime + fireRate)
        {
            FireBullet();
            lastFireTime = Time.time;
        }
    }

    void FireBullet()
    {
        if (bulletPrefab == null || bulletSpawner == null)
        {
            Debug.LogError("Bullet Prefab or Spawner is missing.");
            return;
        }

        GameObject newBullet = Instantiate(bulletPrefab, bulletSpawner.position, bulletSpawner.rotation);
        Rigidbody rb = newBullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = bulletSpawner.forward * 20f;
        }

        Destroy(newBullet, 5f);
    }

    public void JumpPlayer()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }

    void Die()
    {
        Debug.Log("Player has fallen off the platform!");
    }
}
