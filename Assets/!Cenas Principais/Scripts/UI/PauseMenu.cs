using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;

    public bool gameIsPaused;

    [Header("UI")]
    [SerializeField] private RectTransform pauseMenuUI;
    [SerializeField] private GameObject canvasUI;
    [SerializeField] private GameObject canvasInteractions;

    [Header("UI Buttons")]
    [SerializeField] private RectTransform resumeButton, optionsButton, menuButton, quitButton;

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
        pauseMenuUI.gameObject.SetActive(false);
        ResetGrunges();
        canvasInteractions.SetActive(true);
        canvasUI.SetActive(true);
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
        pauseMenuUI.gameObject.SetActive(true);
        canvasInteractions.SetActive(false);
        canvasUI.SetActive(false);
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
