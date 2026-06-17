using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TMPFadeIn : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();
    [SerializeField] private float fadeDuration = 1.5f;

    private void Start()
    {
        FadeIn();
    }

    public void FadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeRoutine());
    }

    private IEnumerator FadeRoutine()
    {
        // 시작 알파 0으로 설정
        foreach (var text in texts)
        {
            if (text != null)
            {
                Color color = text.color;
                color.a = 0f;
                text.color = color;
            }
        }

        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Clamp01(time / fadeDuration);

            foreach (var text in texts)
            {
                if (text != null)
                {
                    Color color = text.color;
                    color.a = alpha;
                    text.color = color;
                }
            }

            yield return null;
        }

        foreach (var text in texts)
        {
            if (text != null)
            {
                Color color = text.color;
                color.a = 1f;
                text.color = color;
            }
        }
    }
}