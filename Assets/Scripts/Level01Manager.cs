using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Audio;

public class Level01Manager : MonoBehaviour
{
    [Header("Camera")]
    public Transform cam;
    public Transform puppyViewPoint;

    [Header("UIs")]
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI instructionText;

    [Header("Pose Listener")]
    public JsListenerLevel01 poseListener;
    public bool atAgreePoint = false;

    [Header("Animator")]
    public Animator puppyAnimator;

    [Header("Sound")]
    public AudioSource puppyAudioSource;
    public AudioClip puppyCrySfx;

    public float camSpeed = 2f;

    private void Start()
    {
        dialogueText.gameObject.SetActive(false);
    }

    public void StartPuppySequence()
    {
        StartCoroutine(PuppySequence());
    }

    IEnumerator PuppySequence()
    {
        instructionText.gameObject.SetActive(false);

        yield return StartCoroutine(MoveCamera());

        yield return StartCoroutine(PlayDialogue());
    }

    IEnumerator MoveCamera()
    {
        Vector3 startPos = cam.position;
        Quaternion startRot = cam.rotation;

        Vector3 targetPos = puppyViewPoint.position;
        Quaternion targetRot = puppyViewPoint.rotation;

        float t = 0f;

        while (t < camSpeed)
        {
            t += Time.deltaTime;

            float lerp = t / camSpeed;

            cam.position =
                Vector3.Lerp(startPos, targetPos, lerp);

            cam.rotation =
                Quaternion.Slerp(startRot, targetRot, lerp);

            yield return null;
        }
    }

    IEnumerator PlayDialogue()
    {
        dialogueText.gameObject.SetActive(true);
        puppyAnimator.SetTrigger("Wake");

        if (puppyAudioSource != null && puppyCrySfx != null)
        {
            puppyAudioSource.PlayOneShot(puppyCrySfx);
        }

        dialogueText.text =
            "Oh!\n앗!";

        yield return new WaitForSeconds(2f);

        dialogueText.text =
            "... Did I just fall asleep?\n... 나 방금 잠에 들었었나?";

        yield return new WaitForSeconds(4f);

        dialogueText.text =
            "Where am I right now? I was supposed to meet my new family!\n나 지금 어디에 있는 거지? 새로운 가족을 만나러 가는 길이었는데!";

        yield return new WaitForSeconds(4f);

        dialogueText.text =
            "... A dream island? I have never heard of it!\n... 꿈의 섬이라고? 처음 들어봐!";

        yield return new WaitForSeconds(4f);

        dialogueText.text =
            "Hey, I should leave to meet my new family.\n저기, 나는 새로운 가족을 만나러 떠나야 해.";

        yield return new WaitForSeconds(4f);

        dialogueText.text =
            "Could you help me find my way out of here?\n내가 여기서 나가게 도와줄 수 있을까?";

        yield return new WaitForSeconds(4f);

        dialogueText.gameObject.SetActive(false);
        instructionText.text = "Make a circle over your head to say yes!\n양손으로 머리 위에 동그라미를 만들어 '네'라고 대답해 주세요!";
        instructionText.gameObject.SetActive(true);
        atAgreePoint = true;
        poseListener.isTriggered2 = false;
    }

    public IEnumerator PlayDialogue2()
    {
        instructionText.gameObject.SetActive(false);
        dialogueText.gameObject.SetActive(true);
        puppyAnimator.SetTrigger("Happy");
        dialogueText.text =
            "Thank you so much! That's so sweet of you.\n진짜 고마워! 너 정말 친절하구나.";

        yield return new WaitForSeconds(3f);

        dialogueText.text =
            "I'll follow you.\n내가 널 따라갈게.";

        yield return new WaitForSeconds(3f);

        dialogueText.gameObject.SetActive(false);
        instructionText.gameObject.SetActive(false);

        yield return StartCoroutine(
            poseListener.LoadScene("Level02")
        );
    }
}