using UnityEngine;

public class TriggerEnter2 : MonoBehaviour
{
    public Level03Manager levelManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("브릿지이벤트시작");
            levelManager.atEvent1Point = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            levelManager.atEvent1Point = false;
        }
    }
}
