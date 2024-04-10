using System.Collections;
using DG.Tweening;
using UnityEngine;

public class BossController : MonoBehaviour
{
    private float jumpingEndSpot = -35f;
    private float jumpingDuration = 1.28f;
    private float jumpDelay = 0.40f;
    public bool playerSeen;
    public Animator animator;
    public bool bossActive;
    public EnemyAI enemyAI;
    public AnimatorOverrideController animatorOverrideController;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!playerSeen)
            {
                ActivateBoss();
                playerSeen = true;
                animator.SetTrigger("Jump");
            }
        }
    }


    public void ActivateBoss()
    {
        transform.DOMoveZ(jumpingEndSpot, jumpingDuration).SetDelay(jumpDelay).OnComplete(() =>
        {
            StartCoroutine(ActivateOtherAnim());
        });
    }

    IEnumerator ActivateOtherAnim()
    {
        yield return new WaitForSeconds(5f);
        bossActive = true;
        enemyAI.enabled = true;
        animator.runtimeAnimatorController = animatorOverrideController;
    }
}
