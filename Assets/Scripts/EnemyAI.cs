using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 5.0f;
    public float attackDistance = 1.5f;
    private bool playerInRange = false;
    private Animator animator;
    private string currentState;

    // Animation States
    const string STATE_IDLE = "Idle";
    const string STATE_RUN = "Run";
    const string STATE_SLASH = "Slash";
    const string STATE_SLASH2 = "Slash2";
    const string STATE_LAUGH = "Laugh";
    public bool laughed;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (laughed)
            return;
        
        if (playerInRange)
        {
            MoveTowardsPlayer();
            LookAtPlayer();
        }
        else
        {
            UpdateMoveAnim(STATE_IDLE);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            playerInRange = true;
            UpdateMoveAnim(STATE_RUN);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == player)
        {
            playerInRange = false;
            UpdateMoveAnim(STATE_IDLE);
        }
    }

    private void MoveTowardsPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
            UpdateMoveAnim(STATE_RUN);
        }
        else
        {
            AttackPlayer();
        }
    }

    private void LookAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * moveSpeed);
    }

    private void AttackPlayer()
    {
        // Choose between Slash and Slash2 based on your game logic
        UpdateMoveAnim(STATE_SLASH); // or STATE_SLASH2
        // Implement attack logic here
        Debug.Log("Attacking the player!");
    }

    private void UpdateMoveAnim(string newState)
    {
        if (currentState == newState) return;
        if (laughed)
            return;
        animator.Play(newState);
        currentState = newState;
    }

    public void Laugh()
    {
        animator.SetTrigger("laugh");
        laughed = true;
    }
}
