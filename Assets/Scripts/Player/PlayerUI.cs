using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

[HideMonoScript]
public class PlayerUI : MonoBehaviour
{
    [Title("UI")]
    [Indent][SerializeField] private GameObject lockedText;
    [Indent][SerializeField] private TextMeshProUGUI promptText;

    public void EnableLockedText(bool i) 
    {
        lockedText.SetActive(i);
    }

    public void EnableInteractionText(bool i)
    {
        promptText.gameObject.SetActive(i);
    }

    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }
}
