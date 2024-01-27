using UnityEngine;

public class LinearMovement : MonoBehaviour
{
    private float speed = 5f;
    void Update()
    {
        transform.position += Vector3.forward * Time.deltaTime * speed;
    }
}
