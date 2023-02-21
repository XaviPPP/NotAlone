using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private GameObject player;
    [SerializeField] private Camera cam;

    [Header("UI")]
    [SerializeField] private GameObject canvasUI;
    [SerializeField] private GameObject canvasEnding;
    [SerializeField] private GameObject canvasInteractions;

    [Header("Controllers")]
    [SerializeField] private GameObject inventoryController;
    [SerializeField] private GameObject pauseController;


    void PlayEndGameAnimation()
    {
        ScriptController.instance.DisableScriptsOnEndGame();

        canvasUI.SetActive(false);
        canvasInteractions.SetActive(false);
        canvasEnding.SetActive(true);
    }
}
