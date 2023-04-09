using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Flashback : MonoBehaviour
{
    [SerializeField] private GameObject flashbacks;
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject subtitle;

    [SerializeField] private string[] subtitles;
    [SerializeField] private string lastSubtitle;

    public void PlayFlashback()
    {
        StartCoroutine(PlayFlashbackCoroutine());
    }

    IEnumerator PlayFlashbackCoroutine()
    {
        flashbacks.SetActive(true);
        background.SetActive(true);
        subtitle.SetActive(true);

        TextMeshProUGUI subtitleText = subtitle.GetComponentInChildren<TextMeshProUGUI>();

        subtitleText.text = string.Empty;

        StartCoroutine(FadeIn(1f, 1f));

        yield return new WaitForSeconds(3f);

        foreach (string s in subtitles)
        {
            subtitleText.text = s;

            yield return new WaitForSeconds(3f);
        }

        subtitleText.text = string.Empty;

        StartCoroutine(FadeOut(0f, 2f));

        yield return new WaitForSeconds(2f);

        background.SetActive(false);
        
        yield return new WaitForSeconds(1f);

        subtitleText.text = lastSubtitle;

        yield return new WaitForSeconds(3f);

        subtitle.SetActive(false);
        flashbacks.SetActive(false);
    }

    IEnumerator FadeIn(float targetAlpha, float fadeTime)
    {
        Image image = background.GetComponent<Image>();
        Color color = image.color;

        while (image.color.a < targetAlpha)
        {
            color.a += Time.deltaTime / fadeTime;
            image.color = color;

            yield return null;
        }
    }

    IEnumerator FadeOut(float targetAlpha, float fadeTime)
    {
        Image image = background.GetComponent<Image>();
        Color color = image.color;

        while (image.color.a > targetAlpha)
        {
            color.a -= Time.deltaTime / fadeTime;
            image.color = color;

            yield return null;
        }
    }
}
