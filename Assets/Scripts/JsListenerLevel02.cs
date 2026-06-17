using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class JsListenerLevel02 : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 3f;

    public Level02Manager levelManager;
    public CameraLerp cam;

    public bool isTriggered = true;

    public void OnPose(string pose)
    {
        if (!cam.sceneStarted)
            return;

        Debug.Log("Detected Pose: " + pose);

        if (pose == "RAIN" && !isTriggered && levelManager.atEventPoint)
        {
            isTriggered = true;

            levelManager.StopRain();
        }
    }

    public void OnWalkStart()
    {
        if (!cam.sceneStarted && !levelManager.atEventPoint)
            return;

        levelManager.isWalking = true;
    }

    public void OnWalkStop()
    {
        if (!cam.sceneStarted || levelManager.rainEventFinished)
            return;

        levelManager.isWalking = false;
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