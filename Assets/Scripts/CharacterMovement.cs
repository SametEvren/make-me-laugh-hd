using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public enum MovementState { Idle, Walk, Run }

    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpForce = 7f;
    public LayerMask groundLayer; // Set this in the inspector
    public Animator animator;
    public MovementState movementState;

    private Rigidbody rb;
    private Vector3 movement;
    private bool isGrounded;
    private string currentState;

    float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;
    public Transform cam;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Input for movement
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        movement = new Vector3(horizontal, 0, vertical).normalized;

        // Check for jump input
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            UpdateMoveAnim("Jump");
            isGrounded = false;
        }

        // Update movement state
        if (isGrounded)
        {
            if (movement.magnitude > 0.1f)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    UpdateMoveAnim("Run");
                    movementState = MovementState.Run;
                }
                else
                {
                    UpdateMoveAnim("Walk");
                    movementState = MovementState.Walk;
                }
            }
            else
            {
                UpdateMoveAnim("Idle");
                movementState = MovementState.Idle;
            }
        }

        UpdateMoveAnim(currentState);
    }

    private void UpdateMoveAnim(string newState)
    {
        if (currentState == newState) return;

        animator.Play(newState);

        currentState = newState;
    }

    void FixedUpdate()
    {
        // Movement
        MoveCharacter(movement);
    }

    void MoveCharacter(Vector3 direction)
    {
        if (direction.magnitude > 0.1f)
        {
            float currentSpeed = movementState == MovementState.Run ? runSpeed : walkSpeed;

            // Move the character
            rb.MovePosition(transform.position + direction * currentSpeed * Time.fixedDeltaTime);

            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity,
                turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = true;
        }
    }
}