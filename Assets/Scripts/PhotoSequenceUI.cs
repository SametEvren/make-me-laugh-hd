using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PhotoSequenceUI : MonoBehaviour
{
    public GameObject[] photos; 
    public float displayTime = 2.0f; 
    public float fadeSpeed = 1.0f; 

    private int currentPhotoIndex = 0;
    
    public void StartShowingPhotos()
    {
        StartCoroutine(ShowPhotos());
    }

    IEnumerator ShowPhotos()
    {
        foreach (var photo in photos)
        {
            photo.SetActive(true);
            yield return StartCoroutine(FadePhoto(photo, 1)); 
            yield return new WaitForSeconds(displayTime);
            yield return StartCoroutine(FadePhoto(photo, 0)); // Fotoğrafı karart
            photo.SetActive(false);
            currentPhotoIndex = (currentPhotoIndex + 1) % photos.Length;
        }
    }

    IEnumerator FadePhoto(GameObject photo, float targetAlpha)
    {
        Image image = photo.GetComponent<Image>();
        float alpha = targetAlpha == 1 ? 0 : 1;
        Color color = image.color;

        while (Mathf.Abs(alpha - targetAlpha) > 0.01f)
        {
            alpha = Mathf.Lerp(alpha, targetAlpha, fadeSpeed * Time.deltaTime);
            color.a = alpha;
            image.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        image.color = color;
    }
}