using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 4f;
    public float sprintSpeed = 7f;
    public float crouchSpeed = 2f;
    public float acceleration = 10f;

    [Header("Jumping & Gravity")]
    public float jumpHeight = 1.5f;
    public float gravity = -20f;
    public float autoJumpCheckDistance = 0.6f;
    public LayerMask obstacleLayer;

    [Header("Crouch")]
    public float crouchHeight = 1f;
    public float standingHeight = 2f;
    public float crouchSmooth = 8f;

    [Header("Ground Check")]
    public float groundCheckDistance = 0.2f;

    [Header("Physics Interaction")]
    public float pushForce = 5f;

    CharacterController controller;
    Vector3 velocity;
    Vector3 moveVelocity;
    bool isGrounded;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        GroundCheck();
        Move();
        Crouch();
        GravityAndJump();
        AutoJump();
    }

    // =============================
    // MOVEMENT
    // =============================
    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 input = transform.right * x + transform.forward * z;
        input.Normalize();

        bool sprint = Input.GetKey(KeyCode.LeftShift);
        bool crouch = Input.GetKey(KeyCode.LeftControl);

        float speed = walkSpeed;
        if (crouch) speed = crouchSpeed;
        else if (sprint) speed = sprintSpeed;

        Vector3 target = input * speed;
        moveVelocity = Vector3.Lerp(moveVelocity, target, acceleration * Time.deltaTime);

        controller.Move(moveVelocity * Time.deltaTime);
    }

    // =============================
    // GRAVITY & JUMP
    // =============================
    void GravityAndJump()
    {
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // =============================
    // GROUND CHECK
    // =============================
    void GroundCheck()
    {
        isGrounded = Physics.Raycast(
            transform.position,
            Vector3.down,
            controller.height / 2 + groundCheckDistance
        );
    }

    // =============================
    // CROUCH
    // =============================
    void Crouch()
    {
        float targetHeight = Input.GetKey(KeyCode.LeftControl) ? crouchHeight : standingHeight;
        controller.height = Mathf.Lerp(controller.height, targetHeight, crouchSmooth * Time.deltaTime);
    }

    // =============================
    // AUTO JUMP
    // =============================
    void AutoJump()
    {
        if (!isGrounded || moveVelocity.magnitude < 0.1f) return;

        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * 0.3f;

        if (Physics.Raycast(origin, transform.forward, out hit, autoJumpCheckDistance, obstacleLayer))
        {
            float obstacleHeight = hit.collider.bounds.max.y - transform.position.y;
            if (obstacleHeight > 0.3f && obstacleHeight < 1.2f)
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    // =============================
    // PUSH RIGIDBODIES
    // =============================
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;

        if (rb == null || rb.isKinematic)
            return;

        if (hit.moveDirection.y < -0.3f)
            return;

        if (moveVelocity.magnitude < 0.1f)
            return;

        Vector3 pushDir = new Vector3(
            hit.moveDirection.x,
            0f,
            hit.moveDirection.z
        ).normalized;

        float speedFactor = Mathf.Clamp01(moveVelocity.magnitude / sprintSpeed);
        Vector3 force = pushDir * pushForce * speedFactor;

        rb.AddForce(force, ForceMode.Force);
    }

    // =============================
    // PUBLIC STATES (FOR CAMERA / AUDIO)
    // =============================
    public bool IsGrounded => isGrounded;
    public bool IsMoving => moveVelocity.magnitude > 0.1f;
    public bool IsSprinting => Input.GetKey(KeyCode.LeftShift);
}
