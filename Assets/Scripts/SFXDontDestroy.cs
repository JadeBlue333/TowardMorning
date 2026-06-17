using UnityEngine;

public class SFXDontDestroy : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sfxClip;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        audioSource.clip = sfxClip;
        audioSource.loop = false;

        Destroy(gameObject, sfxClip.length);
    }
}