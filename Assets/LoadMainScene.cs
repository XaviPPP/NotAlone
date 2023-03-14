using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMainScene : MonoBehaviour
{
    public string sceneName;

    void OnEnable()
    {
        LevelManager.instance.LoadLevelAsync(sceneName);
    }
}
