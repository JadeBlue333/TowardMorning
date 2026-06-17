using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class JsListenerIntro : MonoBehaviour
{
    [Header("Fade")]
    public Image fadeImage;
    public float fadeDuration = 3f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip transitionSfx;

    [Header("UI")]
    public TextMeshProUGUI messageText;

    private bool isTriggered = false;
    private bool isTransitioning = false;

    public void OnPose(string pose)
    {
        Debug.Log("Detected Pose: " + pose);

        if (isTriggered) return;

        if (pose == "START")
        {
            isTriggered = true;

            // 텍스트 변경
            if (messageText != null)
            {
                messageText.text = "Entering the island of dream...\n꿈의 섬에 들어가는 중...";

                // Animator 끄기
                Animator anim = messageText.GetComponent<Animator>();
                if (anim != null)
                {
                    anim.enabled = false;
                }

                // 알파 1로 고정
                Color c = messageText.color;
                c.a = 1f;
                messageText.color = c;
            }

            // 효과음 재생
            if (audioSource != null && transitionSfx != null)
            {
                audioSource.PlayOneShot(transitionSfx);
            }

            StartCoroutine(LoadScene("Level01"));
        }
    }

    public void OnWalkStart() { }
    public void OnWalkStop() { }

    IEnumerator LoadScene(string sceneName)
    {
        isTransitioning = true;

        float t = 0f;
        Color color = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;

            float a = Mathf.Lerp(color.a, 1f, t / fadeDuration);

            fadeImage.color = new Color(
                color.r,
                color.g,
                color.b,
                a
            );

            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}