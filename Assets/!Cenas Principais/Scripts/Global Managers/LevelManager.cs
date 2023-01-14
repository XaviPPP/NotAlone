using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject opacity;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject loadingOpacity;
    public void LoadLevelAsync(string levelName)
    {
        StartCoroutine(LoadSceneAsync(levelName));
    }

    public static void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    IEnumerator LoadSceneAsync(string levelName)
    {
        Debug.Log("Coroutine started");
        opacity.SetActive(true);

        yield return new WaitForSeconds(3);

        loadingPanel.SetActive(true);

        yield return new WaitForSeconds(3);

        AsyncOperation op = SceneManager.LoadSceneAsync(levelName);

        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / .9f);
            Debug.Log(op.progress);

            if (progress > 0.9f)
            {
                loadingOpacity.SetActive(true);

                //yield return new WaitForSeconds(2);

                op.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
