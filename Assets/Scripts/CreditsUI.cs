using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CreditsUI : MonoBehaviour
{
    public List<Transform> names;
    [SerializeField] private PhotoSequenceUI photoSequenceUI;
    public Image backgroundImage;
    public void EnterCredits()
    {
        backgroundImage.DOColor(Color.black, 0.1f);
        names[0].transform.DOLocalMoveX(0, 0.5f).SetEase(Ease.Flash).SetDelay(0.1f);
        names[1].transform.DOLocalMoveX(0, 0.5f).SetEase(Ease.InBounce).SetDelay(0.4f);
        names[2].transform.DOLocalMoveX(0, 0.5f).SetEase(Ease.OutBounce).SetDelay(0.7f);
        names[3].transform.DOLocalMoveX(0, 0.5f).SetEase(Ease.OutElastic).SetDelay(1f);
        names[4].transform.DOLocalMoveX(0, 0.5f).SetEase(Ease.InBack).SetDelay(1.3f).OnComplete(() =>
        {
            GameManager.Instance.skipButton.SetActive(true);
            GameManager.Instance.skipText.DOColor(Color.white, 1f);
            photoSequenceUI.StartShowingPhotos();
        });
    }
}