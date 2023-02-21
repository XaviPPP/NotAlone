using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject interactionText;
    [SerializeField] private GameObject lockedText;
    [SerializeField] private TextMeshProUGUI promptText;

    public void EnableLockedText(bool i) 
    {
        lockedText.SetActive(i);
    }

    public void EnableInteractionText(bool i)
    {
        interactionText.SetActive(i);
    }

    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }
}
