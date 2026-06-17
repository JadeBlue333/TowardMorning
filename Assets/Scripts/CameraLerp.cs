using UnityEngine;
using System.Collections;

public class CameraLerp : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float moveSpeed = 5f;

    [Header("Start Delay")]
    [SerializeField] private bool useDelay = false;
    [SerializeField] private float delayTime = 2f;

    private bool isMoving = false;

    public bool sceneStarted = false;

    private IEnumerator Start()
    {
        if (useDelay)
        {
            yield return new WaitForSeconds(delayTime);
        }

        isMoving = true;
    }

    void Update()
    {
        if (isMoving && targetTransform != null)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                targetTransform.position,
                moveSpeed * Time.deltaTime
            );

            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                targetTransform.rotation,
                moveSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, targetTransform.position) < 0.01f)
            {
                transform.position = targetTransform.position;
                transform.rotation = targetTransform.rotation;
                isMoving = false;
                sceneStarted = true;
            }
        }
    }
}