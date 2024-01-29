using DG.Tweening;
using UnityEngine;

public class LinearMovement : MonoBehaviour
{
    private float speed = 5f;
    public float duration;
    public Transform character;

    private void Start()
    {
        Vector3 forwardDirection = character.transform.forward;
        var startPos = transform.position;

        transform.DOMove(startPos + forwardDirection * 30, duration).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
