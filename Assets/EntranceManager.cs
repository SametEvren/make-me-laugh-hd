
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntranceManager : MonoBehaviour
{
    public bool started;
    public Camera camera;
    public Vector3 finalPos = new Vector3(11.83f, 2.28f, -7.1f);
    public Vector3 finalRot = new Vector3(28.94f, -9.763f, 0);
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !started)
        {
            started = true;
            ChangeCamera();
        }
    }

    public void ChangeCamera()
    {
        camera.transform.DOMove(finalPos, 2f);
        camera.transform.DORotate(finalRot, 2f);
        StartCoroutine(UploadScene());
    }

    IEnumerator UploadScene()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("Game");
    }
}
