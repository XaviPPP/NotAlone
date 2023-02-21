using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevelCaller : MonoBehaviour
{
    public string levelName;

    public void LoadLevel()
    {
        LevelManager.LoadLevel(levelName);
    }
}
