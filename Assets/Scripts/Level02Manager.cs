using UnityEngine;
using TMPro;
using System.Collections;

public class Level02Manager : MonoBehaviour
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
    public JsListenerLevel02 poseListener;

    [Header("Event")]
    public Collider rainPoint;
    public Collider characterCollider;
    public bool atEventPoint = false;
    public ParticleSystem rainParticle;
    private bool particlePlayed = false;
    public float rainFadeDuration = 1f;
    public AudioSource rainAudioSource;
    public AudioClip rainSfx;
    public float maxRainRate = 450f;
    public bool instructionTextShow = false;

    public bool playedDialogue1 = false;
    public bool playedDialogue2 = false;

    public bool rainEventFinished = false;

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
        footStepSource.clip = footStepClip;
        footStepSource.loop = true;

        rainAudioSource.clip = rainSfx;
        rainAudioSource.loop = true;
    }

    void Update()
    {
        //Debug.Log(isWalking);
        //Debug.Log(atEventPoint);
        if (!camState.sceneStarted)
            return;

        puppyAnimator.SetBool("Walk", isWalking);
        humanAnimator.SetBool("Walk", isWalking);

        if (atEventPoint && !rainEventFinished)
        {
            isWalking = false; //강제로 걸음 막기
            puppyAnimator.SetBool("Walk", false);
            humanAnimator.SetBool("Walk", false); //한 번 더 막자

            if (!instructionTextShow)
            {
                //여기서 false해줘야 
                instructionText.gameObject.SetActive(false);
                instructionTextShow = true;
            }
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
            characterGroup.transform.position += Vector3.forward * moveSpeed * Time.deltaTime;

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

    public void StartRain()
    {
        atEventPoint = true;
        isWalking = false;

        if (rainParticle != null && !particlePlayed)
        {
            particlePlayed = true;
            StartCoroutine(FadeInRain());
        }

        if (rainAudioSource != null && !rainAudioSource.isPlaying)
        {
            rainAudioSource.Play();
        }
    }

    public void StopRain()
    {
        if (rainParticle != null && !playedDialogue2)
        {
            StartCoroutine(FadeOutRain());
            playedDialogue2 = true; //미리해두기
        }
    }

    IEnumerator PlayDialogue()
    {
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
            "Oh no! Why is it suddenly raining...\n이런! 왜 갑자기 비가 내리는 걸까...";

        yield return new WaitForSeconds(4f);

        dialogueText.text =
            "It's too cold. what should we do?\n나 너무 추워. 이제 어떻게 하면 좋지?";

        yield return new WaitForSeconds(4f);

        dialogueText.gameObject.SetActive(false);
        instructionText.text = "Can you cover the puppy from the rain?\n강아지가 비를 피하게 도와줄 수 있나요?";
        instructionText.gameObject.SetActive(true);
        poseListener.isTriggered = false;
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

        dialogueText.text = "Oh wow. It stopped! How lucky of us, right?\n우와. 비가 그쳤어! 우리 진짜 운이 좋은데?";

        yield return new WaitForSeconds(4f);

        dialogueText.text = "Let's keep on walking.\n계속 걸어가자.";

        yield return new WaitForSeconds(3f);

        dialogueText.gameObject.SetActive(false);
        instructionText.gameObject.SetActive(false);

        
        puppyAnimator.ResetTrigger("Cry");
        puppyAnimator.SetTrigger("Idle");
        rainEventFinished = true; //콜라이더와 상관없이 이벤트 종료시 캐릭터들끼리 앞으로 걸어나가게 하기 위해서.
        atEventPoint = false;
        isWalking = true;
        onlyAvatarGroup.transform.position += Vector3.forward * moveSpeed * Time.deltaTime;

        yield return StartCoroutine(
            poseListener.LoadScene("Level03"));
    }

    // ------------------------------ 비 페이드인 페이드아웃 효과 ---------------------

    IEnumerator FadeInRain()
    {
        rainParticle.Play();

        var emission = rainParticle.emission;

        float t = 0f;

        while (t < rainFadeDuration)
        {
            t += Time.deltaTime;

            float rate = Mathf.Lerp(
                0f,
                maxRainRate,
                t / rainFadeDuration
            );

            emission.rateOverTime = rate;

            yield return null;
        }

        emission.rateOverTime = maxRainRate;
    }

    IEnumerator FadeOutRain()
    {
        var emission = rainParticle.emission;

        float startRate = emission.rateOverTime.constant;
        float startVolume = rainAudioSource.volume;

        float t = 0f;

        while (t < rainFadeDuration)
        {
            t += Time.deltaTime;

            float normalized = t / rainFadeDuration;

            emission.rateOverTime =
                Mathf.Lerp(startRate, 0f, normalized);

            if (rainAudioSource != null)
            {
                rainAudioSource.volume =
                    Mathf.Lerp(startVolume, 0f, normalized);
            }

            yield return null;
        }

        emission.rateOverTime = 0f;

        rainParticle.Stop();

        if (rainAudioSource != null)
        {
            rainAudioSource.Stop();
        }

        StartCoroutine(PlayDialogue2());
    }
}