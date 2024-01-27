using System;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    //Essentials
    public Transform cam;
    private CharacterController controller;
    private float turnSmoothTime = .1f;
    private float turnSmoothVelocity;

    //Movement
    private Vector2 movement;
    public float walkSpeed;
    public float sprintSpeed;
    private float trueSpeed;

    //Jumping
    public float jumpHeight;
    public float gravity;
    public bool isGrounded;
    private Vector3 velocity;
    public LayerMask layerMask;
    
    //Animation
    private string currentState;
    public Animator animator;
    private const string Idle = "Idle";
    private const string Walk = "Walk";
    private const string Run = "Run";
    private const string Jump = "Jump";

    private void Start()
    {
        trueSpeed = walkSpeed;
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position, .1f, layerMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -1f;
        }
        
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector3 direction = new Vector3(movement.x, 0, movement.y).normalized;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            trueSpeed = sprintSpeed;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            trueSpeed = walkSpeed;
        }
        
        if (direction.magnitude >= 0.1f)
        {
            if (isGrounded)
            {
                if (Math.Abs(trueSpeed - walkSpeed) < 0.1f)
                    UpdateMoveAnim(Walk);
                if (Math.Abs(trueSpeed - sprintSpeed) < 0.1f)
                    UpdateMoveAnim(Run);
            }

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity,
                turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f,angle,0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection.normalized * trueSpeed * Time.deltaTime);
        }
        else
        {
            if (isGrounded)
            {
                UpdateMoveAnim(Idle);
            }
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt((jumpHeight * 10) * -2f * gravity);
            UpdateMoveAnim(Jump);
        }

        if (velocity.y > -20)
        {
            velocity.y += (gravity * 10) * Time.deltaTime;
        }
        controller.Move(velocity * Time.deltaTime);
    }
    
    private void UpdateMoveAnim(string newState)
    {
        if (currentState == newState) return;

        animator.Play(newState);

        currentState = newState;
    }
}