/*
using UnityEngine;

public class TriggerEnter3 : MonoBehaviour
{
    public Level03Manager levelManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("응원이벤트시작");
            levelManager.atEvent2Point = true;
            levelManager.StartCheer();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            levelManager.atEvent2Point = false;
        }
    }
}
*/