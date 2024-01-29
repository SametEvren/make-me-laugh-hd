using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public Transform player;
    private void OnTriggerEnter(Collider other)
    {
        // if (other.CompareTag("Player"))
        // {
        //     if(!player.GetComponent<ThirdPersonController>().isOnSkill)
        //         player.GetComponent<ThirdPersonController>().animator.SetTrigger("BossHit");
        // }
    }
}
