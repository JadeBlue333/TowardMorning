using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class JsListenerLevel01 : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 3f;

    public Level01Manager levelManager;

    public bool isTriggered1 = false;
    public bool isTriggered2 = true;

    public void OnPose(string pose)
    {
        Debug.Log("Detected Pose: " + pose);

        if (pose == "CROUCH" && !isTriggered1)
        {
            isTriggered1 = true;

            levelManager.StartPuppySequence();
        }

        if (pose == "AGREE" && !isTriggered2 && levelManager.atAgreePoint)
        {
            isTriggered2 = true;
            levelManager.atAgreePoint = false;
            StartCoroutine(levelManager.PlayDialogue2());
        }
    }

    public void OnWalkStart() { }
    public void OnWalkStop() { }

    public IEnumerator LoadScene(string sceneName)
    {
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