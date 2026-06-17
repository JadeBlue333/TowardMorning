using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class JsListenerLevel04 : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 3f;

    public Level04Manager levelManager;
    public CameraLerp cam;

    public bool isTriggered1 = true;
    public bool isTriggered2 = true;

    public void OnPose(string pose)
    {
        if (!cam.sceneStarted)
            return;

        Debug.Log("Detected Pose: " + pose);

        if (pose == "CHEER" && !isTriggered1)
        {
            isTriggered1 = true;

            levelManager.DidCheer();
        }

        if (pose == "START" && !isTriggered2)
        {
            isTriggered2 = true;

            levelManager.SayBye();
        }
    }

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