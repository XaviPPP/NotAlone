using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[HideMonoScript]
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public GameObject loadingPanel;
    public GameObject opacity;
    public GameObject loadingOpacity;

    void Awake()
    {
        Debug.Log("awoke");
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void LoadLevelAsync(string levelName)
    {
        StartCoroutine(LoadSceneAsync(levelName));
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void LoadIntro()
    {
        StartCoroutine(LoadIntroScene());
    }

    IEnumerator LoadIntroScene()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Debug.Log("Coroutine started");
        opacity.SetActive(true);

        yield return new WaitForSeconds(2.5f);

        LoadLevel("Intro");
    }

    IEnumerator LoadSceneAsync(string levelName)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Debug.Log("Coroutine started");

        yield return new WaitForSeconds(3f);

        AsyncOperation op = SceneManager.LoadSceneAsync(levelName);

        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / .9f);
            Debug.Log(op.progress);

            if (progress > 0.9f)
            {
                loadingOpacity.SetActive(true);

                yield return new WaitForSeconds(2f);

                op.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
