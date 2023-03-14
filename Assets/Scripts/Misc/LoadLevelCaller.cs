using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevelCaller : MonoBehaviour
{
    public string levelName;

    public void LoadLevel()
    {
        LevelManager.instance.LoadLevel(levelName);
    }

    public void LoadLevelAsync(string levelName)
    {
        LevelManager.instance.LoadLevelAsync(levelName);
    }
}
