using TMPro;
using UnityEngine;

public class PoseListener : MonoBehaviour
{
    public TMP_Text poseText;

    public void OnPose(string pose)
    {
        Debug.Log("Detected Pose: " + pose);

        if (poseText != null)
        {
            poseText.text = pose;
        }
    }
}
