using System;
using System.Collections.Generic;
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
    private const string JugglerBall = "Juggler Ball";
    private const string Talk = "Talk";
    private const string YoYo = "YoYo";
    private const string Card = "Card Throw";
    private const string FlowerGun = "Flower Gun";
    private const string Flute = "Flute";
    private const string HammerCombo1 = "HammerCombo1";
    private const string HammerCombo2 = "HammerCombo2";
    private const string HammerCombo3 = "HammerCombo3";
    public int hammerNumber;
    
    //Skills
    public bool isOnSkill;
    public SkillController skillController;

    public List<GameObject> enemies;
    private void Start()
    {
        trueSpeed = walkSpeed;
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void Update()
    {
        var soundManager = SoundManager.Instance;
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
            if (isGrounded && !isOnSkill)
            {
                if (Math.Abs(trueSpeed - walkSpeed) < 0.1f)
                {
                    UpdateMoveAnim(Walk);
                }

                if (Math.Abs(trueSpeed - sprintSpeed) < 0.1f)
                {
                    UpdateMoveAnim(Run);
                }
            }

            if (!isOnSkill)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity,
                    turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDirection.normalized * trueSpeed * Time.deltaTime);
            }
        }
        else
        {
            if (isGrounded && !isOnSkill)
            {
                UpdateMoveAnim(Idle);
            }
        }

        if (Input.GetButtonDown("Jump") && isGrounded && !isOnSkill)
        {
            velocity.y = Mathf.Sqrt((jumpHeight * 10) * -2f * gravity);
            UpdateMoveAnim(Jump);
        }

        if (Input.GetKeyDown(KeyCode.Q) && !isOnSkill)
        {
            isOnSkill = true;
            UpdateMoveAnim(JugglerBall);
            transform.LookAt(enemies[^1].transform.position);
        }

        if (Input.GetKeyDown(KeyCode.E) && !isOnSkill)
        {
            isOnSkill = true;
            UpdateMoveAnim(Talk);
            transform.LookAt(enemies[^1].transform.position);
        }

        if (Input.GetKeyDown(KeyCode.R) && !isOnSkill)
        {
            isOnSkill = true;
            UpdateMoveAnim(YoYo);
            transform.LookAt(enemies[^1].transform.position);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            isOnSkill = true;
            UpdateMoveAnim(Card);
            transform.LookAt(enemies[^1].transform.position);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            isOnSkill = true;
            UpdateMoveAnim(FlowerGun);
            transform.LookAt(enemies[^1].transform.position);
        }
        
        if (Input.GetKeyDown(KeyCode.U))
        {
            isOnSkill = true;
            UpdateMoveAnim(Flute);
            transform.LookAt(enemies[^1].transform.position);
        }

        if (Input.GetMouseButtonDown(0))
        {
            isOnSkill = true;
            switch (hammerNumber)
            {
                case 1: 
                    UpdateMoveAnim(HammerCombo1);
                    transform.LookAt(enemies[^1].transform.position);
                    break;
                case 2:
                    UpdateMoveAnim(HammerCombo2);
                    transform.LookAt(enemies[^1].transform.position);
                    break;
                case 3:
                    UpdateMoveAnim(HammerCombo3);
                    transform.LookAt(enemies[^1].transform.position);
                    break;
            }
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

        if (currentState == HammerCombo1 || currentState == HammerCombo2 || currentState == HammerCombo3)
        {
            hammerNumber++;
            if (hammerNumber == 4)
                hammerNumber = 1;
        }
        animator.Play(newState);

        currentState = newState;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            foreach (var enemy in enemies)
            {
                if (enemy == other.gameObject)
                    return;
            }
            enemies.Add(other.gameObject);
        }

        if (other.CompareTag("Fog"))
        {
            // other.gameObject.GetComponent<MeshCollider>().enabled = true;
            SoundManager.Instance.combatMusic.gameObject.SetActive(false);
            SoundManager.Instance.bossMusic.gameObject.SetActive(true);
        }
    }
}