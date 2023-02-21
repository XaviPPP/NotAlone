using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    [SerializeField] private GameObject opacity;
    [SerializeField] private TextMeshProUGUI menuButtonText;
    public void LoadMenu()
    {
        StartCoroutine(LoadMainMenu());
    }

    IEnumerator LoadMainMenu()
    {
        opacity.SetActive(true);

        yield return new WaitForSeconds(3);

        SceneManager.LoadScene(0);
    }

    public void ResetFontSize()
    {
        menuButtonText.fontSize = 28;
    }
}
