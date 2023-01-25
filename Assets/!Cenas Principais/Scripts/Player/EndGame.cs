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
        MonoBehaviour[] scripts = player.GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = false;
        }

        MonoBehaviour[] camScripts = cam.GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour script in camScripts)
        {
            script.enabled = false;
        }

        canvasUI.SetActive(false);
        canvasInteractions.SetActive(false);
        canvasEnding.SetActive(true);

        inventoryController.SetActive(false);
        pauseController.SetActive(false);
    }
}
