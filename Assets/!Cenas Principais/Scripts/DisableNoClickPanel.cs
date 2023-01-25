using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableNoClickPanel : MonoBehaviour
{
    [SerializeField] private GameObject noClickPanel;
    public void DisablePanel()
    {
        noClickPanel.SetActive(false);
    }
}
