using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumpscare : MonoBehaviour
{
    [SerializeField] private GameObject smilerUI;
    [SerializeField] private GameObject otherUI;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource heartAudioSource;
    [SerializeField] private AudioClip preScareSound;
    [SerializeField] private AudioClip jumpscareSound;
    [SerializeField] private AudioClip[] scaredSounds;
    [SerializeField] private AudioClip heartBeatSound;
    private bool isPlaying;

    void Start()
    {
        isPlaying = false;
        StartCoroutine(PlayJumpscare());
    }

    void Update()
    {
        //if (1 == UnityEngine.Random.Range(0, 1000) && !isPlaying)
        //{
        //    PlayJumpscare();
        //}
    }

    private IEnumerator PlayJumpscare()
    {
        isPlaying = true;
        audioSource.PlayOneShot(preScareSound);
        yield return new WaitForSeconds(12);
        otherUI.SetActive(false);
        audioSource.PlayOneShot(jumpscareSound);
        smilerUI.SetActive(true);
        yield return new WaitForSeconds(1);
        smilerUI.SetActive(false);
        otherUI.SetActive(true);
        heartAudioSource.PlayOneShot(heartBeatSound);
            audioSource.PlayOneShot(scaredSounds[UnityEngine.Random.Range(0, scaredSounds.Length-1)]);
            yield return new WaitWhile(() => audioSource.isPlaying);
            audioSource.PlayOneShot(scaredSounds[UnityEngine.Random.Range(0, scaredSounds.Length - 1)]);
            yield return new WaitWhile(() => audioSource.isPlaying);
            audioSource.PlayOneShot(scaredSounds[UnityEngine.Random.Range(0, scaredSounds.Length - 1)]);
            yield return new WaitWhile(() => audioSource.isPlaying);
            audioSource.PlayOneShot(scaredSounds[UnityEngine.Random.Range(0, scaredSounds.Length - 1)]);
            yield return new WaitWhile(() => audioSource.isPlaying);
            audioSource.PlayOneShot(scaredSounds[UnityEngine.Random.Range(0, scaredSounds.Length - 1)]);
            yield return new WaitWhile(() => audioSource.isPlaying);
        isPlaying = false;
    }


}
