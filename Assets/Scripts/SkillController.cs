using DG.Tweening;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    public ThirdPersonController thirdPersonController;
    public Transform handPos;
    public GameObject jugglerBall;
    public float randomisation;
    
    //YoYo
    public GameObject yoYoPrefab;
    
    //Card
    public GameObject cardPrefab;
    private Vector3 cardPos = new (0.0063f,0.0071f,0.0558f);
    private Vector3 cardRot = new (-194.706f,20.83701f,-1.962006f);

    public GameObject instantiatedCard;
    
    //Flower Gun
    public GameObject flowerGun;
    
    //Flute
    public GameObject flute;
    
    //Hammer
    public GameObject hammer;


    public SoundManager soundManager;
    public void SetOnSkillFalse()
    {
        thirdPersonController.isOnSkill = false;
        flowerGun.SetActive(false);
        flute.SetActive(false);
        hammer.SetActive(false);
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
                bigJugglerBall.GetComponent<LinearMovement>().character = transform;
                bigJugglerBall.GetComponent<LinearMovement>().enabled = true;
            });
        }
    }

    public void YoYoSkill()
    {
        GameObject yoYo = Instantiate(yoYoPrefab, handPos.position, Quaternion.identity);
        var startPos = yoYo.transform.position;

        Vector3 forwardDirection = transform.forward;

        yoYo.transform.DOMove(startPos + forwardDirection * 10, 0.3f).OnComplete(() =>
        {
            yoYo.transform.DOMove(startPos, 0.4f).OnComplete(() =>
            {
                Destroy(yoYo);
            });
        });
    }

    public void ThrowCardSkill()
    {
        instantiatedCard.GetComponent<LinearMovement>().enabled = true;
        instantiatedCard.GetComponent<LinearMovement>().character = transform;
        instantiatedCard.transform.parent = null;
    }

    public void ActivateCardSkill()
    {
        instantiatedCard = Instantiate(cardPrefab, handPos.position, Quaternion.identity, handPos.transform);
        instantiatedCard.transform.localPosition = cardPos;
        instantiatedCard.transform.localRotation = Quaternion.Euler(cardRot);
    }

    public void ActivateFlowerGun()
    {
        flowerGun.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(1,0);
        flowerGun.SetActive(true);
    }

    public void ShootTheGun()
    {
        float shapeKeyValue = 0;
        DOTween.To(() => shapeKeyValue, x => shapeKeyValue = x, 100, 0.2f)
            .OnUpdate(() => {
                flowerGun.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(1,shapeKeyValue);
            });
    }

    public void ActivateFlute()
    {
        flute.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0,0);
        flute.SetActive(true);
    }

    public void ShootTheFlute()
    {
        flute.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0,0);
        float shapeKeyValue = 0;
        DOTween.To(() => shapeKeyValue, x => shapeKeyValue = x, 100, 1f)
            .OnUpdate(() => {
                flute.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0,shapeKeyValue);
            });
    }

    public void ActivateHammer()
    {
        hammer.SetActive(true);
    }

    public void MakeOpponentLaugh()
    {
        foreach (var enemy in thirdPersonController.enemies)
        {
            if (enemy != null && !enemy.GetComponent<EnemyAI>().laughed)
            {
                if (enemy.GetComponent<EnemyAI>().boss)
                {
                    enemy.GetComponent<EnemyAI>().laughMeter++;
                    if (enemy.GetComponent<EnemyAI>().laughMeter == 5)
                    {
                        enemy.GetComponent<EnemyAI>().Laugh();
                        soundManager.KingLaugh();
                    }
                }
                else
                {
                    enemy.GetComponent<EnemyAI>().Laugh();
                    soundManager.EnemyLaugh();
                    CameraShake.Instance.TriggerShake();
                }
            }
        }
        
    }
}
