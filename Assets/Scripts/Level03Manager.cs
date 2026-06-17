using UnityEngine;
using TMPro;
using System.Collections;

public class Level03Manager : MonoBehaviour
{
    [Header("Camera")]
    public CameraLerp camState;
    public GameObject characterGroup;
    public GameObject onlyAvatarGroup;
    public float moveSpeed = 0.5f;

    [Header("UIs")]
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI instructionText;

    [Header("Pose Listener")]
    public JsListenerLevel03 poseListener;

    [Header("Event")]
    public Collider characterCollider;

    public Collider bridgePoint;
    public bool atEvent1Point = false;
    public Material bridgeMaterial;
    public float bridgeFadeDuration = 2f;
    public AudioSource bridgeAudioSource;
    public AudioClip bridgeSfx;
    public bool bridgeEventFinished = false;

    public bool playedDialogue1 = false;

    /*
    public Collider cheerPoint;
    public bool atEvent2Point = false;

    public bool playedDialogue2 = false;

    public bool atEvent3Point = false;
    */

    [Header("Animator")]
    public Animator puppyAnimator;
    public Animator humanAnimator;
    public bool isWalking = false;

    [Header("Sound")]
    public AudioSource footStepSource;
    public AudioClip footStepClip;
    public AudioSource puppyAudioSource;
    public AudioClip puppyCrySfx;

    private void Start()
    {
        dialogueText.gameObject.SetActive(false);
        instructionText.gameObject.SetActive(false);
        footStepSource.clip = footStepClip;
        footStepSource.loop = true;

        bridgeAudioSource.clip = bridgeSfx;
        bridgeAudioSource.loop = false;
    }

    void Update()
    {
        //Debug.Log(isWalking);
        //Debug.Log(atEventPoint);
        if (!camState.sceneStarted)
            return;

        puppyAnimator.SetBool("Walk", isWalking);
        humanAnimator.SetBool("Walk", isWalking);

        if (atEvent1Point && !bridgeEventFinished)
        {
            isWalking = false; //강제로 걸음 막기
            puppyAnimator.SetBool("Walk", false);
            humanAnimator.SetBool("Walk", false); //한 번 더 막자
            
            if (!playedDialogue1)
            {
                playedDialogue1 = true;
                StartCoroutine(PlayDialogue());
            }
            
            return;
        }

        if (isWalking)
        {
            instructionText.gameObject.SetActive(false);
            characterGroup.transform.position += characterGroup.transform.forward * moveSpeed * Time.deltaTime; //local!

            if (!footStepSource.isPlaying)
            {
                footStepSource.Play();
            }
        }
        else
        {
            instructionText.gameObject.SetActive(true);

            if (footStepSource.isPlaying)
            {
                footStepSource.Stop();
            }
        }
    }

    public void BuildBridge()
    {
        bridgeAudioSource.PlayOneShot(bridgeSfx);
        StartCoroutine(FadeInBridge());
    }

    /*
    public void StartCheer()
    {
        StartCoroutine(PlayDialogue3());
    }
    public void DidCheer()
    {
        StartCoroutine(PlayDialogue4());
    }
    */

    IEnumerator PlayDialogue()
    {
        isWalking = false;

        if (footStepSource.isPlaying)
        {
            footStepSource.Stop();
        }

        puppyAnimator.SetTrigger("Cry");

        if (puppyAudioSource != null && puppyCrySfx != null)
        {
            puppyAudioSource.PlayOneShot(puppyCrySfx);
        }

        dialogueText.gameObject.SetActive(true);

        dialogueText.text =
            "That's the door to the real world, right? Finally! ...\n저게 현실로 가는 문이구나? 드디어! ...";

        yield return new WaitForSeconds(4f);

        dialogueText.text =
            "But looks like we need a bridge...\n그치만 다리가 필요할 것 같은데...";

        yield return new WaitForSeconds(4f);

        dialogueText.text =
            "Maybe I wouldn't make it... Should I give up?\n어쩌면 난 저기에 닿지 못할거야... 포기해야 할까?";

        yield return new WaitForSeconds(5f);

        dialogueText.gameObject.SetActive(false);
        instructionText.text = "Can you make a bridge for the puppy?\n강아지를 위해 다리를 만들어줄 수 있나요?";
        instructionText.gameObject.SetActive(true);
        poseListener.isTriggered1 = false;
    }

    public IEnumerator PlayDialogue2()
    {
        instructionText.gameObject.SetActive(false);
        dialogueText.gameObject.SetActive(true);
        puppyAnimator.SetTrigger("Happy");
        if (puppyAudioSource != null && puppyCrySfx != null)
        {
            puppyAudioSource.PlayOneShot(puppyCrySfx);
        }

        dialogueText.text = "Wow, how did that happen?\n와, 어떻게 된거지?";

        yield return new WaitForSeconds(4f);

        dialogueText.text = "I'm so happy that I didn't give up.\n포기하지 않아서 다행이야.";

        yield return new WaitForSeconds(4f);

        dialogueText.gameObject.SetActive(false);
        instructionText.gameObject.SetActive(false);

        puppyAnimator.ResetTrigger("Happy");
        puppyAnimator.SetTrigger("Idle");
        bridgeEventFinished = true;
        atEvent1Point = false;
        isWalking = true;
        onlyAvatarGroup.transform.position += characterGroup.transform.forward * moveSpeed * Time.deltaTime;

        yield return StartCoroutine(
            poseListener.LoadScene("Level04"));
    }
    /*
    public IEnumerator PlayDialogue3()
    {
        puppyAnimator.SetTrigger("Cry");

        if (puppyAudioSource != null && puppyCrySfx != null)
        {
            puppyAudioSource.PlayOneShot(puppyCrySfx);
        }

        dialogueText.gameObject.SetActive(true);

        dialogueText.text =
            "Now I'm really going to meet my new family.";

        yield return new WaitForSeconds(3f);

        dialogueText.text =
            "This is the moment I have waited for so long...";

        yield return new WaitForSeconds(3f);

        dialogueText.text =
            "But from now on, I have to walk alone, right?";

        yield return new WaitForSeconds(3f);

        dialogueText.gameObject.SetActive(false);
        instructionText.text = "Raise your arms to cheer the puppy up!";
        instructionText.gameObject.SetActive(true);
        poseListener.isTriggered2 = false;
    }

    public IEnumerator PlayDialogue4()
    {
        instructionText.gameObject.SetActive(false);
        dialogueText.gameObject.SetActive(true);
        puppyAnimator.SetTrigger("Happy");
        if (puppyAudioSource != null && puppyCrySfx != null)
        {
            puppyAudioSource.PlayOneShot(puppyCrySfx);
        }

        dialogueText.text = "You are right! I got this!";

        yield return new WaitForSeconds(3f);

        dialogueText.text = "Thank you for helping me out.";

        yield return new WaitForSeconds(3f);

        dialogueText.text = "I could be here thanks to you.";

        yield return new WaitForSeconds(3f);

        dialogueText.gameObject.SetActive(false);
        instructionText.gameObject.SetActive(false);

        puppyAnimator.ResetTrigger("Happy");
        puppyAnimator.SetTrigger("Idle");
        bridgeEventFinished = true;
        atEvent1Point = false;
    }
    */

    IEnumerator FadeInBridge()
    {
        Color startColor = bridgeMaterial.GetColor("_BaseColor");

        float t = 0f;

        while (t < bridgeFadeDuration)
        {
            t += Time.deltaTime;

            Color color = startColor;
            color.a = Mathf.Lerp(0f, 1f, t / bridgeFadeDuration);

            bridgeMaterial.SetColor("_BaseColor", color);

            yield return null;
        }

        Color finalColor = bridgeMaterial.GetColor("_BaseColor");
        finalColor.a = 1f;
        bridgeMaterial.SetColor("_BaseColor", finalColor);

        StartCoroutine(PlayDialogue2());
    }
}