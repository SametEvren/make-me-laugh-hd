using System;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public enum MovementState { Idle, Walk, Run }
    
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float rotationSpeed = 720f; // Degrees per second
    public float jumpForce = 7f;
    public LayerMask groundLayer; // Set this in the inspector
    public Animator animator;
    public MovementState movementState;
    
    private Rigidbody rb;
    private Vector3 movement;
    private bool isGrounded;
    private string currentState;
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
