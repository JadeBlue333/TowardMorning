using UnityEngine;
using TMPro;
using System.Collections;

public class Level04Manager : MonoBehaviour
{
    [Header("Camera")]
    public CameraLerp camState;
    public Camera mainCam;

    [Header("UIs")]
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI instructionText;

    [Header("Pose Listener")]
    public JsListenerLevel04 poseListener;

    [Header("Event")]
    public bool playedDialogue1 = false;

    public Transform puppyAvatar;
    public Transform doorObject;

    public float puppyYRot = 180f;
    public float doorYRot = 90f;

    public float rotationDuration = 2f;

    [Header("Animator")]
    public Animator puppyAnimator;
    public Animator humanAnimator;

    [Header("Sound")]
    public AudioSource doorSource;
    public AudioClip doorClip;
    public AudioSource puppyAudioSource;
    public AudioClip puppyCrySfx;

    private void Start()
    {
        dialogueText.gameObject.SetActive(false);
        instructionText.gameObject.SetActive(false);
        doorSource.clip = doorClip;
        doorSource.loop = false;
    }

    void Update()
    {
        if (!camState.sceneStarted)
            return;

        if (!playedDialogue1)
        {
            StartCoroutine(PlayDialogue());
            playedDialogue1 = true;
        }
    }

    public void DidCheer()
    {
        StartCoroutine(PlayDialogue2());
    }

    public void SayBye()
    {
        StartCoroutine(PlayDialogue3());
    }

    IEnumerator PlayDialogue()
    {
        puppyAnimator.SetTrigger("Cry");

        if (puppyAudioSource != null && puppyCrySfx != null)
        {
            puppyAudioSource.PlayOneShot(puppyCrySfx);
        }

        dialogueText.gameObject.SetActive(true);

        dialogueText.text =
            "Now I'm really going to meet my new family.\n이제 난 진짜로 내 새 가족을 만나러 갈거야.";

        yield return new WaitForSeconds(4f);

        dialogueText.text =
            "This is the moment I have waited for so long...\n이 순간을 정말 오랫동안 기다렸어...";

        yield return new WaitForSeconds(4f);

        dialogueText.text =
            "But from now on, I have to walk alone, right?\n하지만 지금부터는 나 혼자 가야겠지?";

        yield return new WaitForSeconds(4f);

        dialogueText.gameObject.SetActive(false);
        instructionText.text = "Raise your arms to cheer the puppy up!\n양손을 높이 들어 강아지를 응원해주세요!";
        instructionText.gameObject.SetActive(true);
        poseListener.isTriggered1 = false;
    }

    public IEnumerator PlayDialogue2()
    {
        humanAnimator.SetTrigger("Cheer");
        instructionText.gameObject.SetActive(false);
        dialogueText.gameObject.SetActive(true);
        puppyAnimator.SetTrigger("Happy");
        if (puppyAudioSource != null && puppyCrySfx != null)
        {
            puppyAudioSource.PlayOneShot(puppyCrySfx);
        }

        dialogueText.text = "Haha, You are right! I can do this!\n하하, 네 말이 맞아! 난 할 수 있어!";

        yield return new WaitForSeconds(4f);

        dialogueText.text = "Thank you for helping me.\n날 도와줘서 정말 고마워.";

        yield return new WaitForSeconds(4f);

        dialogueText.text = "I wouldn't be here without you.\n네가 없었으면 여기에 오지 못했을거야.";

        yield return new WaitForSeconds(4f);

        dialogueText.gameObject.SetActive(false);
        instructionText.text = "Wave your right hand to say bye.\n오른손을 흔들어 작별인사하세요.";
        instructionText.gameObject.SetActive(true);
        poseListener.isTriggered2 = false;
    }

    public IEnumerator PlayDialogue3()
    {
        instructionText.gameObject.SetActive(false);
        humanAnimator.SetTrigger("Bye");
        puppyAnimator.ResetTrigger("Happy");
        puppyAnimator.SetTrigger("Cry");

        if (puppyAudioSource != null && puppyCrySfx != null)
        {
            puppyAudioSource.PlayOneShot(puppyCrySfx);
        }

        dialogueText.gameObject.SetActive(true);

        dialogueText.text = "Next time, I'll be by your side. I promise.\n다음 번엔, 내가 네 옆에 있을게. 약속할 수 있어.";

        yield return new WaitForSeconds(5f);

        dialogueText.text = "We're never alone.\n우린 혼자가 아니야.";

        yield return new WaitForSeconds(5f);

        dialogueText.text = "And you'll find your own morning.\n너도 너만의 아침을 맞이하게 될 거야";

        yield return new WaitForSeconds(5f);
        StartCoroutine(AniSequence());

        puppyAnimator.SetBool("Walk", true);
        dialogueText.gameObject.SetActive(false);

        if (doorSource != null && doorClip != null)
        {
            puppyAudioSource.PlayOneShot(doorClip);
        }

        yield return new WaitForSeconds(4f);

        yield return StartCoroutine(
            poseListener.LoadScene("Intro"));
    }

    IEnumerator RotateWorldY(
    Transform target,
    float targetY,
    float duration)
    {
        Quaternion startRot = target.rotation;

        Vector3 euler = target.rotation.eulerAngles;
        Quaternion endRot =
            Quaternion.Euler(
                euler.x,
                targetY,
                euler.z);

        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;

            target.rotation =
                Quaternion.Lerp(
                    startRot,
                    endRot,
                    t / duration);

            yield return null;
        }

        target.rotation = endRot;
    }

    IEnumerator AniSequence()
    {
        StartCoroutine(
            RotateWorldY(
                puppyAvatar,
                puppyYRot,
                rotationDuration));

        StartCoroutine(
            RotateWorldY(
                doorObject,
                doorYRot,
                rotationDuration));

        yield return new WaitForSeconds(rotationDuration);

        StartCoroutine(Ending());
    }

    IEnumerator Ending()
    {
        Debug.Log("Ending Start");

        while (true)
        {
            Debug.Log("Moving");

            // 강아지 앞으로
            puppyAvatar.position +=
                puppyAvatar.forward * 0.5f * Time.deltaTime;

            // 카메라 뒤로
            Vector3 camBack = -mainCam.transform.forward;
            camBack.y = 0f;
            camBack.Normalize();

            mainCam.transform.position += camBack * 0.5f * Time.deltaTime;

            yield return null;

            Debug.Log(puppyAvatar.position);
        }
    }
}