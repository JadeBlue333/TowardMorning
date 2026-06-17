using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class JsListenerLevel03 : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 3f;

    public Level03Manager levelManager;
    public CameraLerp cam;

    public bool isTriggered1 = true;
    public bool isTriggered2 = true;

    public void OnPose(string pose)
    {
        if (!cam.sceneStarted)
            return;

        Debug.Log("Detected Pose: " + pose);

        if (pose == "BRIDGE" && !isTriggered1 && levelManager.atEvent1Point)
        {
            isTriggered1 = true;

            levelManager.BuildBridge();
        }
        /*

        if (pose == "CHEER" && !isTriggered2 && levelManager.atEvent2Point)
        {
            isTriggered2 = true;

            //levelManager.StopRain();
        }

        if (pose == "CHEER" && levelManager.atEvent3Point)
        {

            //levelManager.StopRain();
        }
        */
    }

    public void OnWalkStart()
    {
        if (!cam.sceneStarted && !levelManager.atEvent1Point/*|| levelManager.atEvent1Point || levelManager.atEvent2Point || levelManager.atEvent3Point*/)
            return;

        levelManager.isWalking = true;
    }

    public void OnWalkStop()
    {
        if (!cam.sceneStarted || levelManager.bridgeEventFinished /*||levelManager.atEvent1Point || levelManager.atEvent2Point || levelManager.atEvent3Point*/)
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