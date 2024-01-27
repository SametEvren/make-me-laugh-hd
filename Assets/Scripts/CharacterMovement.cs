using System;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    enum MovementState { Idle, Walk, Run }
    
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float rotationSpeed = 720f; // Degrees per second
    public float jumpForce = 7f;
    public LayerMask groundLayer; // Set this in the inspector
    public Animator animator;
    
    private Rigidbody rb;
    private Vector3 movement;
    private bool isGrounded;
    private MovementState currentState = MovementState.Idle;
    private static readonly int Run = Animator.StringToHash("Run");
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int Jump = Animator.StringToHash("Jump");

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Input for movement
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        movement = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;

        // Check for jump input
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetTrigger(Jump);
            isGrounded = false;
        }

        // Update movement state
        if (movement.magnitude > 0.1f)
        {
            currentState = Input.GetKey(KeyCode.LeftShift) ? MovementState.Run : MovementState.Walk;
        }
        else
        {
            currentState = MovementState.Idle;
        }
        
        UpdateMoveAnim(currentState);
    }

    void UpdateMoveAnim(MovementState movementState)
    {
        switch (movementState)
        {
            case MovementState.Idle:
                animator.SetBool(Run,false);
                animator.SetBool(Walk,false);
                break;
            case MovementState.Walk:
                animator.SetBool(Run,false);
                animator.SetBool(Walk,true);
                break;
            case MovementState.Run:
                animator.SetBool(Run,true);
                animator.SetBool(Walk,false);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(movementState), movementState, null);
        }
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
            float currentSpeed = currentState == MovementState.Run ? runSpeed : walkSpeed;

            // Move the character
            rb.MovePosition(transform.position + direction * currentSpeed * Time.fixedDeltaTime);

            // Rotate the character
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.fixedDeltaTime);
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
