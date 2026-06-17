using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFadeOut : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1.5f;

    private void Start()
    {
        FadeOut();
    }

    public void FadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(FadeRoutine());
    }

    private IEnumerator FadeRoutine()
    {
        Color color = fadeImage.color;
        color = Color.black;
        color.a = 1f;
        fadeImage.color = color;

        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;

            float alpha = 1f - Mathf.Clamp01(time / fadeDuration);

            color.a = alpha;
            fadeImage.color = color;

            yield return null;
        }

        color.a = 0f;
        fadeImage.color = color;
    }
}