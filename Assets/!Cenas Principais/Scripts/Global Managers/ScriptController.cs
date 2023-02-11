using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[HideScriptField]
[Serializable]
public class ScriptController : MonoBehaviour
{
    public static ScriptController instance = null;

    [Header("Player")]
    [SerializeField] private GameObject player;
    [SerializeField] private Camera cam;

    [Header("Controllers")]
    [SerializeField] private TimeUpdater _timeUpdater;
    [SerializeField] private AudioManager _audioController;
    [SerializeField] private InventorySystem _inventoryController;
    [SerializeField] private VignetteController _vignetteController;
    [SerializeField] private MessageController _messageController;
    [SerializeField] private PauseMenu _pauseController;
    [SerializeField] private LevelManager _levelController;

    [Space]
    [SerializeField] private MonoBehaviour[] _playerScripts;
    [SerializeField] private MouseLook _mouseLook;


    [Header("Status")]
    [ReadOnly][SerializeField] private bool timeUpdater;
    [ReadOnly][SerializeField] private bool audioController;
    [ReadOnly][SerializeField] private bool inventoryController;
    [ReadOnly][SerializeField] private bool vignetteController;
    [ReadOnly][SerializeField] private bool messageController;
    [ReadOnly][SerializeField] private bool pauseController;
    [ReadOnly][SerializeField] private bool levelController;

    [Space]
    [SerializeField] private bool[] playerScriptsStatus;

    

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
        timeUpdater = _timeUpdater.enabled;
        audioController = _audioController.enabled;
        inventoryController = _inventoryController.enabled;
        vignetteController = _vignetteController.enabled;
        messageController = _messageController.enabled;
        pauseController = _pauseController.enabled;
        levelController = _levelController.enabled;

        for (int i = 0; i < _playerScripts.Length; i++)
        {
            playerScriptsStatus[i] = _playerScripts[i].enabled;
        }
    }

    public void EnablePlayerScript(MonoBehaviour script, bool state)
    {
        foreach (MonoBehaviour m in _playerScripts)
        {
            if (m == script)
            {
                script.enabled = state;
            }
        }
    }

    public void EnableMouseLook(bool state)
    {
        _mouseLook.enabled = state;
    }

    public void EnableTimeUpdater(bool state)
    {
        _timeUpdater.enabled = state;
    }

    public void EnableAudioController(bool state)
    {
        _audioController.enabled = state;
    }

    public void EnableInventoryController(bool state)
    {
        _inventoryController.enabled = state;
    }

    public void EnableVignetteController(bool state)
    {
        _vignetteController.enabled = state;
    }

    public void EnableMessageController(bool state)
    {
        _messageController.enabled = state;
    }

    public void EnablePauseController(bool state)
    {
        _pauseController.enabled = state;
    }

    public void EnableLevelController(bool state)
    {
        _levelController.enabled = state;
    }
}
