using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartGame : MonoBehaviour
{
    [SerializeField] private GameObject startGame;
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject subtitle;

    [SerializeField] private string subtitleText;

    void Start()
    {
        PlayStartGame();
    }

    public void PlayStartGame()
    {
        StartCoroutine(StartGameCoroutine());
    }

    IEnumerator StartGameCoroutine()
    {
        startGame.SetActive(true);
        background.SetActive(true);
        subtitle.SetActive(true);

        StartCoroutine(FadeOut(0f, 3f));

        yield return new WaitForSeconds(5f);

        TextMeshProUGUI subtitleTMP = subtitle.GetComponentInChildren<TextMeshProUGUI>();

        subtitleTMP.text = subtitleText;

        yield return new WaitForSeconds(3f);

        startGame.SetActive(false);
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
