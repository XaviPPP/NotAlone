using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[HideMonoScript]
public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;

    [HideInInspector] public bool gameIsPaused;

    [Title("UI")]
    [Indent][SerializeField] private RectTransform pauseMenu;
    [Indent][SerializeField] private GameObject UI;
    [Indent][SerializeField] private GameObject interactions;

    [Title("UI Buttons")]
    [Indent][SerializeField] private RectTransform resumeButton;
    [Indent][SerializeField] private RectTransform optionsButton;
    [Indent][SerializeField] private RectTransform menuButton;
    [Indent][SerializeField] private RectTransform quitButton;

    private void Awake()
    {
        // If there is not already an instance of SoundManager, set it to this.
        if (instance == null)
        {
            instance = this;
        }
        //If an instance already exists, destroy whatever this object is to enforce the singleton.
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gameIsPaused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!InventorySystem.instance.isClosed)
            {
                InventorySystem.instance.CloseInv();
                Cursor.lockState = CursorLockMode.Locked;
                return;
            }

            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenu.gameObject.SetActive(false);
        ResetGrunges();
        interactions.SetActive(true);
        UI.SetActive(true);
        Time.timeScale = 1f;
        gameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        AudioListener.pause = false;
    }

    private void ResetGrunges()
    {
        resumeButton.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0f);
        resumeButton.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.white;

        optionsButton.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0f);
        optionsButton.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.white;

        menuButton.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0f);
        menuButton.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.white;

        quitButton.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0f);
        quitButton.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.white;
    }

    public void Pause()
    {
        pauseMenu.gameObject.SetActive(true);
        interactions.SetActive(false);
        UI.SetActive(false);
        Time.timeScale = 0f;
        gameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        AudioListener.pause = true;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
