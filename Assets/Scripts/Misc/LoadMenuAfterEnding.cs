using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[HideScriptField]
public class LoadMenuAfterEnding : MonoBehaviour
{
    public string sceneName;
    private void OnEnable()
    {
        LevelManager.LoadLevel(sceneName);
        Cursor.lockState = CursorLockMode.None;
    }
}
