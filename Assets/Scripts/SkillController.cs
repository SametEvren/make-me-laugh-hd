using DG.Tweening;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    public ThirdPersonController thirdPersonController;
    public Transform handPos;
    public GameObject jugglerBall;
    public float randomisation;
    
    public void SetOnSkillFalse()
    {
        thirdPersonController.isOnSkill = false;
    }

    public void JugglerBallSkill()
    {
        for (int i = 0; i < 40; i++)
        {
            var randAddition = new Vector3(Random.Range(-randomisation, randomisation), Random.Range(-randomisation, randomisation), Random.Range(-randomisation, randomisation));
            var jBall = Instantiate(jugglerBall, handPos.position + randAddition, Quaternion.identity);
            jBall.transform.localScale = Vector3.one * 0.1f;
            var x = jBall;
            x.transform.DOMove(handPos.transform.position, 2f).OnComplete(() =>
            {
                Destroy(x);
                var bigJugglerBall = Instantiate(jugglerBall, handPos.position, Quaternion.identity);
                bigJugglerBall.GetComponent<LinearMovement>().enabled = true;
            });
        }
    }
}
