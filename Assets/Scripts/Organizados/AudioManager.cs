using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void PlayClip(AudioSource audioSource, AudioClip clip, float volume)
    {
        audioSource.PlayOneShot(clip, volume);
    }

    public void PlayFadeOut(AudioSource audioSource, float fadeTime)
    {
        StartCoroutine(FadeOut(audioSource, fadeTime));
    }

    public void PlayFadeIn(AudioSource audioSource, float fadeTime)
    {
        StartCoroutine(FadeIn(audioSource, fadeTime));
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

    private IEnumerator FadeIn(AudioSource audioSource, AudioClip clip, float FadeTime)
    {
        audioSource.PlayOneShot(clip);
        audioSource.volume = 0f;
        while (audioSource.volume < 1)
        {
            audioSource.volume += Time.deltaTime / FadeTime;
            yield return null;
        }
    }

    private IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        audioSource.Play();
        audioSource.volume = 0f;
        while (audioSource.volume < 1)
        {
            audioSource.volume += Time.deltaTime / FadeTime;
            yield return null;
        }
    }
}
