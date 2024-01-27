using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    
    public float speed = 6f;
    
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    
    public bool isGrounded;
    private string currentState;
    public LayerMask groundLayer; // Set this in the inspector
    public Animator animator;
    public MovementState movementState; 
    public float walkSpeed = 5f;
    public float runSpeed = 10f;

    public enum MovementState { Idle, Walk, Run }

    private void Update()
    {
        Debug.Log(controller.isGrounded);
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity,
                turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            float currentSpeed = movementState == MovementState.Run ? runSpeed : walkSpeed;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
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
    
    private void UpdateMoveAnim(string newState)
    {
        if (currentState == newState) return;

        animator.Play(newState);

        currentState = newState;
    }
}
