using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[HideMonoScript]
[Serializable]
public class ScriptController : MonoBehaviour
{
    public static ScriptController instance = null;

    [Title("Player")]
    [Indent][SerializeField] private GameObject player;
    [Indent][SerializeField] private Camera cam;

    [Title("Managers")]
    [Indent][SerializeField] private TimeUpdater _timeManager;
    [Indent][SerializeField] private AudioManager _audioManager;
    [Indent][SerializeField] private InventorySystem _inventoryManager;
    [Indent][SerializeField] private VignetteController _vignetteManager;
    [Indent][SerializeField] private MessageController _messageManager;
    [Indent][SerializeField] private PauseMenu _pauseManager;
    [Indent][SerializeField] private LevelManager _levelManager;


    [Title("Player scripts")]
    [Indent][SerializeField] private MonoBehaviour[] _playerScripts;
    [Indent][SerializeField] private MouseLook _mouseLook;


    [Title("Status")]
    [Indent][ReadOnly][SerializeField] private bool timeManager;
    [Indent][ReadOnly][SerializeField] private bool audioManager;
    [Indent][ReadOnly][SerializeField] private bool inventoryManager;
    [Indent][ReadOnly][SerializeField] private bool vignetteManager;
    [Indent][ReadOnly][SerializeField] private bool messageManager;
    [Indent][ReadOnly][SerializeField] private bool pauseManager;
    [Indent][ReadOnly][SerializeField] private bool levelManager;

    [Space]
    [Indent][SerializeField] private bool[] playerScriptsStatus;

    

    // Start is called before the first frame update
    private void Awake()
    {
        // If there is not already an instance of ScriptManager, set it to this.
        if (instance == null)
        {
            instance = this;
        }
        //If an instance already exists, destroy whatever this object is to enforce the singleton.
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        //Set SCriptManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _playerScripts = player.GetComponents<MonoBehaviour>();
        _mouseLook = cam.GetComponent<MouseLook>();
        
        playerScriptsStatus = new bool[_playerScripts.Length];
    }

    private void Update()
    {
        UpdateStatus();
    }

    private void UpdateStatus()
    {
        timeManager = _timeManager.enabled;
        audioManager = _audioManager.enabled;
        inventoryManager = _inventoryManager.enabled;
        vignetteManager = _vignetteManager.enabled;
        messageManager = _messageManager.enabled;
        pauseManager = _pauseManager.enabled;
        levelManager = _levelManager.enabled;

        for (int i = 0; i < _playerScripts.Length; i++)
        {
            playerScriptsStatus[i] = _playerScripts[i].enabled;
        }
    }

    public void EnablePlayerScript(Type script, bool state)
    {
        foreach (MonoBehaviour m in _playerScripts)
        {
            if (m.GetType() == script)
            {
                m.enabled = state;
            }
        }
    }

    public void DisableScriptsOnEndGame()
    {
        foreach (MonoBehaviour m in _playerScripts)
        {
            m.enabled = false;
        }

        EnableInventoryController(false);
        EnablePauseController(false);
        EnableMessageController(false);
    }

    public void EnableMouseLook(bool state)
    {
        _mouseLook.enabled = state;
    }

    public void EnableTimeUpdater(bool state)
    {
        _timeManager.enabled = state;
    }

    public void EnableAudioController(bool state)
    {
        _audioManager.enabled = state;
    }

    public void EnableInventoryController(bool state)
    {
        _inventoryManager.enabled = state;
    }

    public void EnableVignetteController(bool state)
    {
        _vignetteManager.enabled = state;
    }

    public void EnableMessageController(bool state)
    {
        _messageManager.enabled = state;
    }

    public void EnablePauseController(bool state)
    {
        _pauseManager.enabled = state;
    }

    public void EnableLevelController(bool state)
    {
        _levelManager.enabled = state;
    }
}
