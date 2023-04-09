using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

[HideMonoScript]
public class UiManager : MonoBehaviour
{
    public static UiManager instance;


    [Title("UI Items")]
    [SerializeField] private GameObject vignette;
    [SerializeField] private GameObject UI;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject interactions;
    [SerializeField] private GameObject ending;
    [SerializeField] private GameObject death;
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject subtitle;
    [SerializeField] private GameObject sleep;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EnableVignette(bool i)
    {
        vignette.SetActive(i);
    }

    public void EnableUI(bool i)
    {
        UI.SetActive(i);
    }

    public void EnableMenu(bool i)
    {
        menu.SetActive(i);
    }

    public void EnableInteractions(bool i)
    {
        interactions.SetActive(i);
    }

    public void EnableEnding(bool i)
    {
        ending.SetActive(i);
    }

    public void EnableDeath(bool i)
    {
        death.SetActive(i);
    }

    public void EnableInventory(bool i)
    {
        inventory.SetActive(i);
    }

    public void EnableSubtitle(bool i)
    {
        subtitle.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Empty;
        subtitle.SetActive(i);
    }

    public void EnableSleep(bool i)
    {
        sleep.SetActive(i);
    }

    public void EnableAll(bool i)
    {
        vignette.SetActive(i);
        UI.SetActive(i);
        menu.SetActive(i);
        interactions.SetActive(i);
        ending.SetActive(i);
        death.SetActive(i);
        inventory.SetActive(i);
        sleep.SetActive(i);
    }

    public void ShowSubtitle(string text)
    {
        EnableSubtitle(true);
        
        TextMeshProUGUI subtitleText = subtitle.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        subtitleText.text = text;
    }

}
