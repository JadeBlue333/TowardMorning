using UnityEngine;

public class TriggerEnter : MonoBehaviour
{
    public Level02Manager levelManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            levelManager.atEventPoint = true;
            levelManager.StartRain();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            levelManager.atEventPoint = false;
        }
    }
}
