using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableNoClickPanel : MonoBehaviour
{
    [SerializeField] private GameObject noClickPanel;
    [SerializeField] private GameObject menuButton;
    public void DisablePanel()
    {
        noClickPanel.SetActive(false);
    }

    public void EnableMenuButton()
    {
        menuButton.SetActive(true);
    }
}
