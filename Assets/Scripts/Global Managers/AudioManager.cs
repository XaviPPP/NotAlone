using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HideMonoScript]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    [Title("Audio sources")]
    [Indent][SerializeField] private AudioSource windSource;
    [Indent][SerializeField] private AudioSource damageSource;
    [Indent][SerializeField] private AudioSource lowHealthLoopSource;
    [Indent][SerializeField] private AudioSource jumpscareSource;
    [Indent][SerializeField] private AudioSource heartBeatSource;
    [Indent][SerializeField] private AudioSource deathSource;
    [Indent][SerializeField] private AudioSource ambienceSource;
    [Indent][SerializeField] private AudioSource pickupSource;

    [Title("Clips")]
    [Indent][SerializeField] private AudioClip[] pickupClips;
    [Indent][SerializeField] private AudioClip[] dropClips;

    // Start is called before the first frame update
    private void Awake()
    {
        // If there is not already an instance of SoundManager, set it to this.
        if (instance == null)
        {
            instance = this;
        }
        //If an instance already exists, destroy whatever this object is to enforce the singleton.
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }

    public void PlayRandomPickupClip()
    {
        pickupSource.PlayOneShot(pickupClips[UnityEngine.Random.Range(0, pickupClips.Length)]);
    }

    public void PlayRandomDropClip()
    {
        pickupSource.PlayOneShot(dropClips[UnityEngine.Random.Range(0, dropClips.Length)]);
    }

    public void PlayWindClip(AudioClip clip, bool loop = true)
    {
        windSource.clip = clip;
        windSource.loop = loop;
        PlayFadeIn(windSource, 3f, 0.7f);
    }

    public void StopPlayingWindClip()
    {
        windSource.Stop();
    }

    public void PlayDamageClip(AudioClip clip)
    {
        damageSource.PlayOneShot(clip);
    }

    public void PlayLowHealthLoopClip(AudioClip clip)
    {
        windSource.clip = clip;
        windSource.loop = true;
        PlayFadeIn(lowHealthLoopSource, 2f);
    }

    public void StopLowHealthLoopClip()
    {
        PlayFadeOut(lowHealthLoopSource, 2f);
    }

    public void PlayJumpscareClip(AudioClip clip)
    {
        jumpscareSource.PlayOneShot(clip);
    }

    public void PlayHeartBeatClip(AudioClip clip)
    {
        heartBeatSource.PlayOneShot(clip);
    }

    public void PlayDeathClip(AudioClip clip)
    {
        deathSource.PlayOneShot(clip);
    }

    public AudioSource GetDeathAudioSource()
    {
        return deathSource;
    }

    public void PlayAmbienceClip(AudioClip clip, bool loop = false)
    {
        deathSource.clip = clip;
        deathSource.loop = loop;
        deathSource.Play();
    }

    public void PlayClip(AudioSource audioSource, AudioClip clip, float volume)
    {
        audioSource.PlayOneShot(clip, volume);
    }

    public void PlayClipLoop(AudioSource audioSource, AudioClip clip, float volume)
    {
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.volume = volume;
        audioSource.Play();
    }

    public void PlayFadeOut(AudioSource audioSource, float fadeTime)
    {
        StartCoroutine(FadeOut(audioSource, fadeTime));
    }

    public void PlayFadeIn(AudioSource audioSource, float fadeTime)
    {
        StartCoroutine(FadeIn(audioSource, fadeTime));
    }

    public void PlayFadeIn(AudioSource audioSource, float fadeTime, float volume)
    {
        StartCoroutine(FadeIn(audioSource, fadeTime, volume));
    }

    public void PlayFadeIn(AudioSource audioSource, AudioClip clip, float fadeTime)
    {
        StartCoroutine(FadeIn(audioSource, clip, fadeTime));
    }

    private IEnumerator FadeOut(AudioSource audioSource, float fadeTime)
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
        audioSource.Stop();
    }

    private IEnumerator FadeIn(AudioSource audioSource, AudioClip clip, float FadeTime, float volume = 1f)
    {
        audioSource.PlayOneShot(clip);
        audioSource.volume = 0f;
        while (audioSource.volume < volume)
        {
            audioSource.volume += Time.deltaTime / FadeTime;
            yield return null;
        }
    }

    private IEnumerator FadeIn(AudioSource audioSource, float FadeTime, float volume = 1f)
    {
        audioSource.Play();
        audioSource.volume = 0f;
        while (audioSource.volume < volume)
        {
            audioSource.volume += Time.deltaTime / FadeTime;
            yield return null;
        }
    }
}
