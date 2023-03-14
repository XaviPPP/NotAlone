using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLoadingScreen : MonoBehaviour
{
    public string sceneName;
    
    private void OnEnable()
    {
        LevelManager.instance.LoadLevel(sceneName);
    }
}
